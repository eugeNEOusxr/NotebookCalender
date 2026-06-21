import java.io.File
import org.apache.tools.ant.taskdefs.condition.Os
import org.gradle.api.DefaultTask
import org.gradle.api.GradleException
import org.gradle.api.logging.LogLevel
import org.gradle.api.tasks.Input
import org.gradle.api.tasks.TaskAction

/**
 * Patched for Windows: use cmd /c + npx.cmd (not `npm run -- tauri` — no such npm script).
 */
open class BuildTask : DefaultTask() {
    @Input
    var rootDirRel: String? = null
    @Input
    var target: String? = null
    @Input
    var release: Boolean? = null

    @TaskAction
    fun assemble() {
        runTauriCli(resolveNpxExecutable())
    }

    private fun resolveNpxExecutable(): String {
        System.getenv("NPX_BIN")?.trim()?.takeIf { it.isNotEmpty() }?.let { return it }
        System.getenv("NPM_BIN")?.trim()?.takeIf { it.isNotEmpty() }?.let { path ->
            val npx = path.replace("npm.cmd", "npx.cmd").replace("npm.exe", "npx.exe")
            if (File(npx).isFile) return npx
        }

        if (Os.isFamily(Os.FAMILY_WINDOWS)) {
            val candidates = listOf(
                "C:\\Program Files\\nodejs\\npx.cmd",
                "${System.getenv("ProgramFiles")}\\nodejs\\npx.cmd"
            )
            for (path in candidates) {
                if (File(path).isFile) return path
            }
            return "npx.cmd"
        }
        return "npx"
    }

    fun runTauriCli(executable: String) {
        val rootDirRel = rootDirRel ?: throw GradleException("rootDirRel cannot be null")
        val target = target ?: throw GradleException("target cannot be null")
        val release = release ?: throw GradleException("release cannot be null")

        val cliArgs = mutableListOf(
            "tauri",
            "android",
            "android-studio-script"
        )
        if (project.logger.isEnabled(LogLevel.DEBUG)) {
            cliArgs.add("-vv")
        } else if (project.logger.isEnabled(LogLevel.INFO)) {
            cliArgs.add("-v")
        }
        if (release) {
            cliArgs.add("--release")
        }
        cliArgs.addAll(listOf("--target", target))

        project.exec {
            workingDir(File(project.projectDir, rootDirRel))
            if (Os.isFamily(Os.FAMILY_WINDOWS)) {
                commandLine(listOf("cmd", "/c", executable) + cliArgs)
            } else {
                executable(executable)
                args(cliArgs)
            }
        }.assertNormalExitValue()
    }
}
