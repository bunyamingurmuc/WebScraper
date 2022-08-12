using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using WebScraperAPI.Context;
using WebScraperAPI.Model;

namespace WebScraperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CharacterController : ControllerBase
    {

        private readonly ScrapperContext _context;

        public CharacterController(ScrapperContext context)
        {
            _context = context;
        }

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
                    imageUrl = node.QuerySelector("tr td div img")
                    .GetAttributeValue("data-src", ""),
                    descriptionUrl = $"https://attackontitan.fandom.com/wiki/{name.Replace(" ", "_")}_(Anime)",

                };
            });
            return Ok(data);

        }
        [HttpGet]
        [Route("/[controller]/[action]")]
        public async Task<ActionResult> getProducts()
        {
            Category category = new Category();


            List<string> level1Category = new List<string>();
            List<string> level2Category = new List<string>();
            List<string> level3Category = new List<string>();
            List<string> level3Filter = new List<string>();
            List<string> lvl1urls = new List<string>();
            List<string> lvl2urls = new List<string>();
            List<string> lvl3urls = new List<string>();
            List<string> productlinks = new List<string>();
            List<string> productFeatures = new List<string>();

            List<string> productNames = new List<string>();


            var url = "https://www.n11.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            IList<HtmlNode> nodes = doc.QuerySelectorAll("li.catMenuItem");

            foreach (var node in nodes)
            {
                var catName = node.QuerySelector("a span").InnerText;
                level1Category.Add(catName);

            }

            for (int i = 0; i < level1Category.Count; i++)
            {
                lvl1urls.Add($"https://www.n11.com/{level1Category[i].Replace(" &amp; ", "-").Replace(", ", "-").ToLower()}");
            }

            for (int i = 0; i < level1Category.Count; i++)
            {
                if (_context.Categories.Select(i => i.CategoryName).Contains(level1Category[i]))
                {

                }
                else
                {
                  await  _context.Categories.AddAsync(new Category()
                    {
                        CategoryLevel = 1,
                        CategoryName = level1Category[i],
                        CategoryLink = lvl1urls[i],
                    });
                }

            }
           await _context.SaveChangesAsync();

            foreach (var lvl1url1 in lvl1urls)
            {
                HtmlWeb web2 = new HtmlWeb();
                HtmlDocument doc2 = web.Load(lvl1url1);
                IList<HtmlNode> nodes1 = doc2.QuerySelectorAll("li.mainCat");
                foreach (var node1 in nodes1)
                {
                    var cat2Name = node1.QuerySelector("a").InnerText;
                    level2Category.Add(cat2Name);
                    var clearedUrlName = $"{url}{cat2Name.Replace(" &amp; ", "-ve-").Replace(", ", "-").Replace(" ", "-").ToLower().Replace("þ", "s").Replace("ö", "o").Replace("ð", "g").Replace("ý", "i").Replace("Ý", "i").Replace("ç", "c").Replace("ü", "u")}";
                    lvl2urls.Add(clearedUrlName);
                  await  _context.Categories.AddAsync(new Category()
                    {
                        CategoryName = cat2Name,
                        CategoryLink= clearedUrlName,
                         ParentId= _context.Categories.Where(x=>x.CategoryLink==lvl1url1).SingleOrDefaultAsync()?.Id
                    }
                    );
                }
            }
          
          await  _context.SaveChangesAsync();


            foreach (var urllvl2 in lvl2urls)
            {
                HtmlWeb web3 = new HtmlWeb();
                HtmlDocument doc3 = web.Load(urllvl2);
                IList<HtmlNode> nodes4 = doc3.QuerySelectorAll("section.filterCategory ul.filterList li.filterItem");
                foreach (var node4 in nodes4)
                {
                    var cat3Name = node4.QuerySelector("a").InnerText;
                    var clearedLvl3CatName = cat3Name.Replace("\n", "").Trim();
                    var clearedLvl3CatUrl = $"{urllvl2}/{cat3Name.Replace("\n", "").Trim().Replace(" &amp; ", "-ve-").Replace(", ", "-").Replace(" ", "-").ToLower().Replace("þ", "s").Replace("ö", "o").Replace("ð", "g").Replace("ý", "i").Replace("Ý", "i").Replace("ç", "c").Replace("ü", "u")}";
                    level3Category.Add(clearedLvl3CatName);
                    lvl3urls.Add(clearedLvl3CatUrl);
                    if (!(_context.Categories.Select(i => i.CategoryLink).Contains(clearedLvl3CatUrl))) 
                    {
                        _context.Categories.Add(new Category
                        {
                            CategoryName = clearedLvl3CatName,
                            CategoryLink = clearedLvl3CatUrl,
                            ParentId = _context.Categories.Where(x => x.CategoryLink == urllvl2).Take(1).SingleOrDefaultAsync()?.Id
                        });
                    }
                    
                    
                }
            }
          await  _context.SaveChangesAsync();
            foreach (var lvl2url in lvl2urls)
            {
                HtmlWeb web3 = new HtmlWeb();
                HtmlDocument doc3 = web3.Load(lvl2url);
                IList<HtmlNode> nodes3 = doc3.QuerySelectorAll("section.filter h2");
                foreach (var node3 in nodes3)
                {
                    var filter3 = node3.QuerySelector("h2").InnerText;
                    level3Filter.Add(filter3);
                    if (_context.Categories.Where(x => x.CategoryLink == lvl2url).SingleOrDefault()!=null)
                    {
                        
                        _context.Categories.Where(x => x.CategoryLink == lvl2url).Take(1).SingleOrDefault()?.Filters?.Add(new Filter
                        {
                            Name = filter3,

                        });
                    }
                    
                }
            }
            _context.SaveChanges();



            //foreach (var lvl3url in lvl3urls)
            //{
            //    HtmlWeb web4 = new HtmlWeb();
            //    HtmlDocument doc4 = web4.Load(lvl3url);
            //    IList<HtmlNode> nodes5 = doc4.QuerySelectorAll("div.pro ");
            //    foreach (var node5 in nodes5)
            //    {
            //        var link = node5.QuerySelector("a").GetAttributeValue("href", "");
            //        productlinks.Add(link);

            //    }
            //}
            //foreach (var productlink in productlinks)
            //{
            //    HtmlWeb web5 = new HtmlWeb();
            //    HtmlDocument doc5 = web5.Load(productlink);
            //    IList<HtmlNode> nodes6 = doc5.QuerySelectorAll("div.feaItem");
            //    foreach (var node6 in nodes6)
            //    {
            //        var featureName= node6.QuerySelector("span:nth-child(1)").InnerText;
            //        var featureDescription= node6.QuerySelector("span:nth-child(2)")==null? "": node6.QuerySelector("span:nth-child(2)").InnerText;
            //        productFeatures.Add($"{featureName}: {featureDescription}");
            //    }
            //}
            var quaryCategories = _context.Categories.AsQueryable().Include(x => x.ParentCategory).Include(x => x.Categories).Include(x => x.Filters);
            var listedCategories = await quaryCategories.ToListAsync();
            return Ok(listedCategories);
        }

    }


}
// nested table,recursive function,parent data same table