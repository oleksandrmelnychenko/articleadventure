using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using MVC.ArticleAdventure.Helpers;

namespace MVC.ArticleAdventure.Controllers
{
    public class TagsController : Controller
    {
        private readonly ILogger<TagsController> _logger;
        private readonly ITagService _tagService;

        public TagsController(ILogger<TagsController> logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }
        [HttpGet]
        public async Task<IActionResult> Tags()
        {
            TagsModel tagsModel = await GetAllMainTags();
            return View(tagsModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMainTag(TagsModel tagsModel)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            if (tagsModel.AddMainTag.Name == null)
            {
                tagsModel = await GetAllMainTags();
                return View("Tags", tagsModel);
            }
            await _tagService.AddMainTag(tagsModel.AddMainTag, token);
            return Redirect("~/Tags/Tags");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeMainTag(TagsModel tagsModel)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            if (tagsModel.AddMainTag.Name == null)
            {
                tagsModel = await GetAllMainTags();
                return View("Tags", tagsModel);
            }
            await _tagService.ChangeMainTag(tagsModel.AddMainTag, token);

            return Redirect("~/Tags/Tags");
        }

        [HttpPost]
        public async Task<IActionResult> AddSupTag(TagsModel tagsModel)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            if (tagsModel.SelectMainTag == null)
            {
                tagsModel = await GetAllMainTags();
                return View("Tags",tagsModel);
            }
            tagsModel.AddSupTag.IdMainTag = tagsModel.SelectMainTag.Id;
            await _tagService.AddSupTag(tagsModel.AddSupTag, token);
            return Redirect("~/Tags/Tags");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSupTag(TagsModel tagsModel)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            if (tagsModel.AddSupTag.Name == null)
            {
                tagsModel = await GetAllMainTags();
                return View("Tags", tagsModel);
            }
            tagsModel.AddSupTag.IdMainTag = tagsModel.AddMainTag.Id;
            await _tagService.ChangeSupTag(tagsModel.AddSupTag, token);

            return Redirect("~/Tags/Tags");
        }

        [HttpGet]
        public async Task<IActionResult> GetSupTag(Guid ChangeSupTagsNetUid)
        {
            var supTag = await _tagService.GetSupTag(ChangeSupTagsNetUid);
            supTag.IsSelected = true;
            TagsModel tagsModel = await GetAllMainTags();
            
            foreach (var tag in tagsModel.MainTags)
            {
                if (tag.SubTags.Any())
                {
                    var clickedSupTag = tag?.SubTags.FirstOrDefault(x => x.NetUid == supTag.NetUid);
                    if (clickedSupTag!= null)
                    {
                        tag.SubTags.First(x=>x.NetUid == clickedSupTag.NetUid).IsSelected = true;
                    }
                }
            } 
            return View("Tags", tagsModel);

        }

        [HttpGet]
        public async Task<IActionResult> SetChangeSupArticle(Guid SetChangeSupArticleNetUid)
        {
            var supTag = await _tagService.GetSupTag(SetChangeSupArticleNetUid);
            supTag.IsSelected = true;
            TagsModel tagsModel = await GetAllMainTags();
            tagsModel.AddSupTag = supTag;


            foreach (var tag in tagsModel.MainTags)
            {
                if (tag.SubTags.Any())
                {
                    var clickedSupTag = tag?.SubTags.FirstOrDefault(x => x.NetUid == supTag.NetUid);
                    if (clickedSupTag != null)
                    {
                        tag.SubTags.First(x => x.NetUid == clickedSupTag.NetUid).IsSelected = true;
                    }
                }
            }
            return View("Tags", tagsModel);
        }

        [HttpGet]
        public async Task<IActionResult> SetChangeMainArticle(Guid SetChangeMainArticleNetUid)
        {
            var mainTag = await _tagService.GetMainTag(SetChangeMainArticleNetUid);
            mainTag.IsSelected = true;

            TagsModel tagsModel = await GetAllMainTags();
            tagsModel.AddMainTag = mainTag;
            tagsModel.MainTags.Find(x => x.NetUid == mainTag.NetUid).IsSelected = true;
            return View("Tags", tagsModel);

        }
        [HttpGet]
        public async Task<IActionResult> SelectChangeMainArticle(Guid SelectChangeMainArticleNetUid)
        {
            var mainTag = await _tagService.GetMainTag(SelectChangeMainArticleNetUid);
            mainTag.IsSelected = true;

            TagsModel tagsModel = await GetAllMainTags();
            tagsModel.SelectMainTag = mainTag;
            return View("Tags", tagsModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetMainTag(Guid ChangeMainTagsNetUid)
        {
            var mainTag = await _tagService.GetMainTag(ChangeMainTagsNetUid);
            mainTag.IsSelected = true;

            TagsModel tagsModel = await GetAllMainTags();
            tagsModel.MainTags.Find(x => x.NetUid == mainTag.NetUid).IsSelected = true;

            return View("Tags", tagsModel);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveSupTag(Guid RemoveSupTagsNetUid)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            await _tagService.RemoveSupTag(RemoveSupTagsNetUid, token);
            TagsModel tagsModel = await GetAllMainTags();

            return View("Tags", tagsModel);
        }


        [HttpGet]
        public async Task<IActionResult> RemoveMainTag(Guid RemoveMainTagsNetUid)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            await _tagService.RemoveMainTag(RemoveMainTagsNetUid, token);
            TagsModel tagsModel = await GetAllMainTags();

            return View("Tags", tagsModel);
        }

        [NonAction]
        private async Task<TagsModel> GetAllMainTags()
        {
            var AllTags = await _tagService.GetAllTags();
            TagsModel tagsModel = new TagsModel { MainTags = AllTags.Data };
            return tagsModel;
        }
    }
}
