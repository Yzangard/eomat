using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using eZet.EveLib.EveCrestModule;
using eZet.EveLib.EveCrestModule.Models.Resources;
using eZet.EveLib.EveCrestModule.Models.Resources.Market;
using System.Windows.Media;
using DevExpress.XtraPrinting.Native;
using System.Text.RegularExpressions;
namespace EVE_Market.Helpers
{
    
    public class EveApiWrapper
    {
        private const string BaseUri = "https://public-crest.eveonline.com/types/";
        readonly EveCrest _crest = new EveCrest();

        public string getItemUri(long itemId)
        {
            return string.Format("{0}{1}/", BaseUri, itemId);
        }

        public async Task<MarketGroupCollection> GetMarketGroups()
        {
            return await _crest.GetRoot().QueryAsync(r => r.MarketGroups).ConfigureAwait(false);
        }

        public async Task<MarketTypeCollection> GetMarketTypes()
        {
            return await _crest.GetRoot().QueryAsync(r => r.MarketTypes).ConfigureAwait(false);
        }

        public async Task<RegionCollection> GetRegions()
        {
            return await _crest.GetRoot().QueryAsync(r => r.Regions).ConfigureAwait(false);
        }

        public async Task<Region> GetRegion(string regionName)
        {
            return await GetRegions().Result.QueryAsync(r => r.Items.Single(s => s.Name == regionName)).ConfigureAwait(false);
        }

        public async Task<ConstellationCollection> GetConstellations()
        {
            return await _crest.GetRoot().QueryAsync(r => r.Constellations).ConfigureAwait(false);
        }

        public async Task<Constellation> GetConstellation(string constellationName)
        {
            return await GetConstellations().Result.QueryAsync(r => r.Items.Single(s => s.Name == constellationName)).ConfigureAwait(false);
        }

        public async Task<List<Constellation>> GetConstellations(string region)
        {
            var root = await _crest.GetRootAsync().ConfigureAwait(false);
            var regions = await root.QueryAsync(r => r.Regions).ConfigureAwait(false);
            var tmpRegion = await regions.QueryAsync(r => r.Items.Single(s => s.Name == region)).ConfigureAwait(false);

            var response = new List<Constellation>();

            foreach (var constellation in tmpRegion.Constellations)
            {
                var tmpConstellation = await(await
                    _crest.GetRoot()
                        .QueryAsync(r => r.Constellations).ConfigureAwait(false))
                        .QueryAsync(r => r.Items.Single(a => a.Id == constellation.InferredId)).ConfigureAwait(false);

                response.Add(tmpConstellation);
            }

            return response;
        }

        public async Task<SystemCollection> GetSystems()
        {
            return await _crest.GetRoot().QueryAsync(r => r.Systems).ConfigureAwait(false);
        }

        public async Task<SolarSystem> GetSystem(string systemName)
        {
            return await GetSystems().Result.QueryAsync(r => r.Items.Single(a => a.Name == systemName)).ConfigureAwait(false);
        }

        public async Task<MarketOrderCollection> GetRegionOrders(string region, long itemId)
        {
            return
                (await
                    (await
                        (await _crest.GetRootAsync())
                            .QueryAsync(r => r.Regions))
                        .QueryAsync(x => x.Single(r => r.Name == region)))
                    .Query(r => r.MarketOrders, "type", getItemUri(itemId));
        }

        public List<MarketOrderCollection> GetRegionOrders(string region, List<long> itemsId)
        {
            return itemsId.Select(itemId => GetRegionOrders(region, itemId).Result).ToList();
        }

        public async Task<MarketOrderCollection> GetRegionOrders(string region, string itemName)
        {
            // get the item
            var item = (await
                (await
                    (await _crest.GetRootAsync())
                        .QueryAsync(r => r.ItemTypes))
                    .QueryAsync(r => r.Items.Single(a => a.Name == itemName)));
            
            
            // get the orders, passing our item to the final Query()
            return await
                (await
                    (await
                        (await _crest.GetRootAsync())
                            .QueryAsync(r => r.Regions))
                        .QueryAsync(r => r.Items.Single(a => a.Name == region)))
                    .QueryAsync(r => r.MarketOrders, item);
        }
    }
}
