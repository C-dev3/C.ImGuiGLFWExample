using C.ImGuiGLFW;
using C.ImGuiGLFWExample.Windows;
using Hexa.NET.ImGui;

internal class Program
{
    private static readonly DemoWindow demoWindow = new("DemoTitle");
    private static void Main()
    {
        ImGuiController.Initialize($"TESTWINDOW", C.ImGuiGLFW.Data.Module.Nodes, C.ImGuiGLFW.Data.Module.Plot);
        ImGuiController.OnMainMenuBarRender += MainMenuBar;
        ImGuiController.OnWindowFocus += WindowFocus;
        ImGuiController.AddWindow(demoWindow);
        ImGuiController.Run();
    }

    private static void WindowFocus(bool focused)
    {
        Console.WriteLine($"WindowFocus: {focused}");
    }

    private static void MainMenuBar()
    {
        if (ImGui.BeginMenu("Windows"))
        {
            if (ImGui.MenuItem("Window Toggle"))
            {
                demoWindow.Toggle();
            }
            ImGui.EndMenu();
        }
    }
}