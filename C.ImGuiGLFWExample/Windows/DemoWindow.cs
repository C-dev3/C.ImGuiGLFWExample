using C.ImGuiGLFW.Common;
using C.ImGuiGLFW.ImGuiMethods;
using Hexa.NET.ImGui;
using System.Numerics;

namespace C.ImGuiGLFWExample.Windows;

internal class DemoWindow : Window
{
    private float progress = 0.5f;
    private readonly Vector2 barSize = new(400, 15);
    private readonly HierarchyViewer hierarchyViewer = new(true);
    private readonly List<string> sampleList = [];
    private readonly DragDropList<string> list = new(multiSelectEnabled: true);

    public DemoWindow(string name) : base(name)
    {
        Flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize;
        hierarchyViewer.AddRootNodes(CreateSampleHierarchy());
        for (int i = 0; i < 1000; i++) { sampleList.Add($"SampleItem {i + 1}"); }
        list.AddItems(sampleList);
    }

    public override void PreDraw()
    {
        var mainViewport = ImGui.GetMainViewport();
        WindowSize = mainViewport.WorkSize;
        Position = mainViewport.WorkPos;
    }

    protected override void Draw()
    {
        progress = (float)ImGui.GetTime() % 10f / 10f;
        ImGuiC.BufferingBar("Loading##buffer1", progress, barSize);
        ImGuiC.BufferingBarRounded("Loading##buffer2", progress, barSize);
        ImGuiC.ProgressBar("Progress##progress1", progress, barSize);
        ImGuiC.ProgressBarRounded("Progress##progress2", progress, barSize);
        ImGuiC.LinearProgressBar(progress, barSize);

        ImGuiC.Spinner("Spinner", 60.0f);
        ImGui.SameLine();
        ImGuiC.LoadingCircle(60.0f);
        ImGui.SameLine();
        ImGuiC.CircularProgress(progress, 60.0f);

        list.Render((item, index) => $"{index}: {item}");
        hierarchyViewer.Render();
    }

    public override void OnDispose()
    {
        hierarchyViewer.OnNodeSelected -= OnNodeSelected;
        hierarchyViewer.OnNodeVisibilityChanged -= OnNodeVisibilityChanged;
        hierarchyViewer.OnNodeMoved -= OnNodeMoved;
    }

    private void OnNodeSelected(HierarchyNode node)
    {
        Console.WriteLine($"Selected: {node.Name}");
        if (node.Data is not null && node.Data is string str)
        {
            Console.WriteLine($"Data:{str}");
        }
    }

    private void OnNodeVisibilityChanged(HierarchyNode node, bool isVisible) => Console.WriteLine($"{node.Name} visibility changed to: {isVisible}");

    private void OnNodeMoved(HierarchyNode movedNode, HierarchyNode? newNode, int newIndex) => Console.WriteLine($"{movedNode.Name} | {newNode?.Name ?? "null"} | {newIndex}");

    private static List<HierarchyNode> CreateSampleHierarchy()
    {
        var root = new List<HierarchyNode>();
        var obj = new HierarchyNode("Object");
        root.Add(obj);
        var volume = new HierarchyNode("Volume");
        root.Add(volume);
        var dirLight = new HierarchyNode("Directional Light");
        root.Add(dirLight);
        var skybox = new HierarchyNode("Skybox");
        root.Add(skybox);
        root.Add(new HierarchyNode("Object 1"));
        root.Add(new HierarchyNode("Object 2"));
        var spotlight = new HierarchyNode("Spotlight", false);
        root.Add(spotlight);
        var iblLight = new HierarchyNode("IBL Light", false);
        root.Add(iblLight);

        root.Add(new HierarchyNode("Object 3"));
        var ambientLight = new HierarchyNode("Ambient Light") { HasVisibilityToggle = false };
        root.Add(ambientLight);
        var pointLight = new HierarchyNode("Point Light") { HasVisibilityToggle = false };
        root.Add(pointLight);

        var object4 = new HierarchyNode("Object 4");
        object4.Children.Add(new HierarchyNode("Object 5"));
        root.Add(object4);

        var object6 = new HierarchyNode("Object 6");
        var object7 = new HierarchyNode("Object 7");
        object7.Children.Add(new HierarchyNode("Object 9"));
        var object8 = new HierarchyNode("Object 8");
        object8.Children.Add(new HierarchyNode("Object 10") { Data = "UserDataSample" });
        object6.Children.Add(object7);
        object6.Children.Add(object8);
        root.Add(object6);

        return root;
    }
}
