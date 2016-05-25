using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using eomat.Helpers;
using eomat.Helpers.Interface;
using eomat.ViewModels;
using EVE_Market.Helpers;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace eomat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NodeManager _locationNodes = new NodeManager();
        private readonly NodeManager _marketItemNodes = new NodeManager();
        private readonly EveApiWrapper _eaw = new EveApiWrapper();
        private List<string> _selectedRegions = new List<string>();

        public MainWindow()
        {
            InitializeComponent();



            using (var ele = new eveLocationEntities())
            {
                #region Regions
                var regions = ele.mapRegions;
                regions.Load();

                foreach (var region in regions)
                {
                    var node = new RegionNode
                    {
                        Name = region.regionName,
                        NodeID = region.regionID,
                        IsSelected = false
                    };

                    _locationNodes.Nodes.Add(node);
                }
                #endregion

                #region Systems
                var systems = ele.mapSolarSystems;
                systems.Load();

                foreach (var system in systems)
                {
                    var node = new SystemNode
                    {
                        Name = system.solarSystemName,
                        NodeID = system.solarSystemID,
                        ParentNodeID = system.regionID,
                        IsFiltered = false
                    };

                    _locationNodes.Nodes.Add(node);
                }
                #endregion
            }

            using (var eie = new eveInventoryEntities())
            {
                #region Market Groups
                var groups = eie.invMarketGroups;
                groups.Load();

                //var groups = _eaw.GetMarketGroups().Result;

                foreach (var group in groups)
                {
                    //var tmp = @group.ParentGroup?.InferredId ?? 0;
                    var node = new MarketGroupNode
                    {
                        Name = group.marketGroupName,
                        NodeID = group.marketGroupID,
                        ParentNodeID = group.parentGroupID
                    };

                    _marketItemNodes.Nodes.Add(node);
                }
                #endregion

                #region Market Items

                var items = eie.invTypes;
                items.Load();

                //var items = _eaw.GetMarketTypes().Result;

                foreach (var item in items)
                {
                    switch (item.marketGroupID)
                    {
                        case 2155:
                            continue;
                        case null:
                            continue;
                        case 354496:
                            continue;
                        case 354341:
                            continue;
                        case 356922:
                            continue;
                        case 354395:
                            continue;
                        case 354396:
                            continue;
                    }

                    var node = new MarketItemNode
                    {
                        Name = item.typeName,
                        NodeID = item.typeID + 2000000000000000,
                        ParentNodeID = item.marketGroupID
                    };

                    _marketItemNodes.Nodes.Add(node);
                }

                #endregion
            }

            locationTreeView.ItemsSource = _locationNodes.Nodes;
            locationTreeView.Columns["Name"].SortOrder = ColumnSortOrder.Ascending;

            locationTreeView.View.NodeChanged += NodeChecked;


            marketItemTreeView.ItemsSource = _marketItemNodes.Nodes;
            marketItemTreeView.Columns["Name"].SortOrder = ColumnSortOrder.Ascending;
        }

        private void NodeChecked(object sender, TreeListNodeChangedEventArgs e)
        {
            if (e.ChangeType == NodeChangeType.CheckBox)
            {                
                var tmp = (RegionNode)e.Node.Content;

                if (e.Node.IsChecked != null) AddCheckedRegions(tmp.Name, e.Node.IsChecked.Value);
            }
        }

        private void AddCheckedRegions(string name, bool isAdded)
        {
            if (isAdded)
            {
                _selectedRegions.Add(name);
            }
            else
            {
                _selectedRegions.Remove(name);
            }            
        }
    }
}