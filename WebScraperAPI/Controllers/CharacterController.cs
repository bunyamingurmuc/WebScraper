using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;

namespace WebScraperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> get()
        {
            var url = "https://attackontitan.fandom.com/wiki/List_of_characters/Anime";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            IList<HtmlNode> nodes = doc.QuerySelectorAll("div.characterbox-main")[1]
                .QuerySelectorAll("div.characterbox-container table tbody"); 
            var data = nodes.Select((node) =>
            {
                var name = node.QuerySelector("tr:nth-child(2) th a").InnerText;
                return new
                {
                    name = name,
                    imageUrl=node.QuerySelector("tr td div img")
                    .GetAttributeValue("data-src",""),
                    descriptionUrl=$"https://attackontitan.fandom.com/wiki/{name.Replace(" ","_")}_(Anime)",
                    
                };
            });
            return Ok(data);
            Console.WriteLine("dedede");
        }
        [HttpGet]
        [Route("/[controller]/[action]")]
        public async Task<ActionResult> getProducts()
        {
            List<string> level1Category = new List<string>();
            List<string> level2Category = new List<string>(); 
            List<string> level3Category = new List<string>(); 
            List<string> level3Filter = new List<string>();
            List<string> lvl1urls= new List<string>();
            List<string> lvl2urls= new List<string>();
            List<string> lvl3urls= new List<string>();

            var url = "https://www.n11.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            IList<HtmlNode> nodes = doc.QuerySelectorAll("li.catMenuItem");
            foreach (var node in nodes)
            {
                var catName= node.QuerySelector("a span").InnerText;
                level1Category.Add(catName);
            }
            for (int i = 0; i < level1Category.Count; i++)
            {
                lvl1urls.Add($"https://www.n11.com/{level1Category[i].Replace(" &amp; ","-" ).Replace(", ","-").ToLower()}");
            }
            foreach (var urllvl2 in lvl1urls)
            {
                HtmlWeb web2 = new HtmlWeb();
                HtmlDocument doc2 = web.Load(urllvl2);
                IList<HtmlNode> nodes1 = doc2.QuerySelectorAll("li.mainCat");
                foreach (var node1 in nodes1)
                {
                    var cat2Name = node1.QuerySelector("a").InnerText;
                    level2Category.Add(cat2Name);
                    lvl2urls.Add($"{url}{cat2Name.Replace(" &amp; ", "-ve-").Replace(", ", "-").Replace(" ", "-").ToLower().Replace("þ", "s").Replace("ö", "o").Replace("ð","g").Replace("ý", "i").Replace("Ý","i").Replace("ç","c").Replace("ü","u")}");}
            }

            foreach (var lvl2url in lvl2urls)
            {
                HtmlWeb web3 = new HtmlWeb();
                HtmlDocument doc3 = web3.Load(lvl2url);
                IList<HtmlNode> nodes3 = doc3.QuerySelectorAll("section.filter h2");
                foreach (var node3 in nodes3)
                {
                    var filter3 = node3.QuerySelector("h2").InnerText;
                    level3Filter.Add(filter3);
                }
            }
            return Ok(level3Filter);
        }
        
    };

       
    
}