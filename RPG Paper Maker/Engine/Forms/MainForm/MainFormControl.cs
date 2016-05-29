﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPG_Paper_Maker
{
    public class MainFormControl
    {
        public void InitializeMain()
        {
            // Creating RPG Paper Maker Games folder
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "RPG Paper Maker Games");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // This code created the basic tree map nodes settings
            /*
            TreeNode rootNode, directoryNode, mapNode;
            rootNode = this.TreeMap.Nodes.Add("Maps");
            rootNode.Tag = TreeTag.CreateRoot();
            directoryNode = rootNode.Nodes.Add("Plains");
            directoryNode.Tag = TreeTag.CreateDirectory();
            mapNode = directoryNode.Nodes.Add("MAP0001");
            mapNode.Tag = TreeTag.CreateMap("MAP0001");
            WANOK.SaveTree(this.TreeMap, "TreeMapDatas.rpmdatas");
            */

            // This code created the basic first map settings
            /*
            WANOK.SaveDatas(new MapInfos(25, 25), "infos.map");
            */

            // Getting engine settings
            WANOK.Settings = WANOK.LoadDatas<EngineSettings>(WANOK.PATHSETTINGS);

            // Updating special infos
            WANOK.InitializeKeyBoard();
            WANOK.ABSOLUTEENGINEPATH = Path.GetDirectoryName(WANOK.ExcecutablePath);
        }

        // -------------------------------------------------------------------
        // SetTitle
        // -------------------------------------------------------------------

        public void SetTitle(string name, string dir)
        {
            WANOK.ProjectName = name;
            WANOK.CurrentDir = dir;
        }

        // -------------------------------------------------------------------
        // OpenNewDialog
        // -------------------------------------------------------------------

        public void OpenNewDialog()
        {
            WANOK.InitializeKeyBoard();
        }

        // -------------------------------------------------------------------
        // CloseProject
        // -------------------------------------------------------------------

        public void CloseProject()
        {
            WANOK.ProjectName = null;
            WANOK.CurrentDir = ".";
        }


        // -------------------------------------------------------------------
        // SaveTreeMap()
        // -------------------------------------------------------------------

        public void SaveTreeMap(TreeView treeMap)
        {
            WANOK.SaveTree(treeMap, Path.Combine(new string[] { WANOK.CurrentDir, "Content", "Datas", "Maps", "TreeMapDatas.rpmdatas" }));
        }

        // -------------------------------------------------------------------
        // FindRootNode
        // -------------------------------------------------------------------

        public TreeNode FindRootNode(TreeNode treeNode)
        {
            while (treeNode.Parent != null)
            {
                treeNode = treeNode.Parent;
            }
            return treeNode;
        }

        // -------------------------------------------------------------------
        // IsAChild
        // -------------------------------------------------------------------

        public bool IsATreeChild(TreeNode node, TreeNode parent)
        {
            while (node.Parent != null)
            {
                if (node == parent) return true;
                node = node.Parent;
            }
            return false;
        }
    }
}
