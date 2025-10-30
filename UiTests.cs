
using System.Collections.Generic;
using Carbon.Components;
using Carbon.Extensions;
using Oxide.Game.Rust.Cui;
using UnityEngine;

namespace Carbon.Plugins;

[Info("UiTests", "Corvus", "0.0.2")]
[Description("It facilitates UI testing stuff.")]
public class UiTests : CarbonPlugin {
    private string _uiPath;
    private List<string> _uiFiles = new();
    protected readonly CUI.Handler Handler = new();

    private void Init() {
        _uiPath = System.IO.Path.Combine("carbon", "Data", "UiTests");

        if (!OsEx.Folder.Exists(_uiPath)) {
            OsEx.Folder.Create(_uiPath);
            return;
        }

        var files = OsEx.Folder.GetFilesWithExtension(_uiPath, ".json");

        foreach(var file in files) {
            _uiFiles.Add(System.IO.Path.GetFileNameWithoutExtension(file));
        }

        if (_uiFiles.Count == 0) {
            Puts("No UI test files found in the UiTests folder.");
        }
    }

    private void OnServerInitialized() {

    }

    private void Unload() {
        CmdUnloadUI();
    }

    [ConsoleCommand("uitests.done")]
    private void CmdUnloadUI() {
        CuiHelper.DestroyUi(BasePlayer.activePlayerList[0], "UiRootPanel");
    }

    [ConsoleCommand("uitests")]
    private void CmdLoadUI(ConsoleSystem.Arg args) {
        CmdUnloadUI();

        if ( args.GetString(0, "") is not string ui || !_uiFiles.Contains(ui) ) {
            args.ReplyWith($"Usage: uitests <{string.Join(" | ", _uiFiles)}>");
            return;
        }
        
        string filePath = System.IO.Path.Combine("carbon", "Data", "UiTests", $"{ui}.json");

        if (OsEx.File.Exists(filePath)) {
            var json = OsEx.File.ReadText(filePath);

            CommunityEntity.ServerInstance.ClientRPC(RpcTarget.Player("AddUI", args.Connection), json.ToString());
        }
    }

    private bool HorizExpanded = false;
    private bool VertExpanded = false;

    [ConsoleCommand("uitests.toggleeh")]
    private void CmdToggleHExpand(ConsoleSystem.Arg args) {
        var player = args.Player();

        HorizExpanded = !HorizExpanded;

        var panel = new CuiElementContainer() {
            { new CuiElement() { Name = "H_Panel", Update = true, Components = { new CuiHorizontalLayoutGroupComponent() { ChildForceExpandHeight = HorizExpanded } } } },
            { new CuiElement() { Name = "ExpandHButtonLabel", Update = true, Components = { new CuiTextComponent() { Text = HorizExpanded ? "SHRINK HEIGHT" : "EXPAND HEIGHT" } } } }
        };

        CuiHelper.AddUi(player, panel);
    }

    [ConsoleCommand("uitests.toggleew")]
    private void CmdToggleVExpand(ConsoleSystem.Arg args) {
        var player = args.Player();

        VertExpanded = !VertExpanded;

        var panel = new CuiElementContainer() {
            { new CuiElement() { Name = "H_Panel", Update = true, Components = { new CuiHorizontalLayoutGroupComponent() { ChildForceExpandWidth = VertExpanded } } } },
            { new CuiElement() { Name = "ExpandWButtonLabel", Update = true, Components = { new CuiTextComponent() { Text = VertExpanded ? "SHRINK WIDTH" : "EXPAND WIDTH" } } } }
        };

        CuiHelper.AddUi(player, panel);
    }


    private bool ElemActive = true;

    [ConsoleCommand("uitests.inactive")]
    private void CmdToggleInactive(ConsoleSystem.Arg args) {
        var player = args.Player();

        ElemActive = !ElemActive;

        var panel = new CuiElementContainer() {
            { new CuiElement() { Name = "H_Item_B", Update = true, ActiveSelf = ElemActive } },
            { new CuiElement() { Name = "HideButtonLabel", Update = true, Components = { new CuiTextComponent() { Text = ElemActive ? "HIDE SECOND" : "SHOW SECOND" } } } }
        };

        CuiHelper.AddUi(player, panel);
    }
}
