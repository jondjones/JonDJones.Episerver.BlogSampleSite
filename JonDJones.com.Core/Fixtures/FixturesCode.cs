
//using EPiServer.Core;
//using EPiServer.DataAccess;
//using EPiServer.Security;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using JonDJones.Com.Core.Pages;
//using EPiServer.ServiceLocation;

//namespace JonDJones.Com.Core
//{
//    public class FixturesCode
//    {
//        private IEpiServerDependencies _epiServerDependencies;

//        private ContentAssetHelper _contentAssetHelper;

//        public FixturesCode(IEpiServerDependencies epiServerDependencies, ContentAssetHelper contentAssetHelper)
//        {
//            _epiServerDependencies = epiServerDependencies;
//            _contentAssetHelper = contentAssetHelper;
//        }

//        public ContentReference CreateHomePage(string pageName)
//        {
//            try
//            {
//                var existingContentPage =
//                    _epiServerDependencies.ContentRepository.GetChildren<StartPage>(
//                    _epiServerDependencies.ContextResolver.RootPage);

//                if (existingContentPage != null && existingContentPage.Where(x => x.PageName == pageName).Any())
//                    return null;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }

//            var contentPage =
//                _epiServerDependencies.ContentRepository.GetDefault<StartPage>
//                (_epiServerDependencies.ContextResolver.RootPage);

//            contentPage.Name = pageName;
//            contentPage.PageTitle = pageName;
//            contentPage.SeoTitle = pageName;
//            contentPage.Keywords = pageName;
//            contentPage.Description = new XhtmlString(pageName);
//            contentPage.Author = pageName;

//            contentPage.MainTitle = "Your Favorite Source of EpiServer Tips N Tricks";
//            contentPage.MainDescription = new XhtmlString("Your Source for Admin Plugins, Blocks, Commerce, CSS Tutorials, Customizing EpiServers UIm Content Areas, Dynamic Properties, EpiServer Api Explained, Getting An EpiServer Job, Getting Started Advice, Initialization Module, Multilingual Websites, Page Types, Performance Tips, Properties, Routing, Scheduled Tasks, Selection Factories, Structuring Your Project, Troubleshooting Tips, Unit Testing");
            

//            return _epiServerDependencies.ContentRepository.Save(contentPage, SaveAction.Publish, AccessLevel.NoAccess);
//        }

//        public ContentReference CreateBlogHomePage()
//        {
        
//            var pageName = "Blog";

//            try
//            {
//                var existingBlogHomePage =
//                    _epiServerDependencies.ContentRepository.GetChildren<BlogHomePage>(
//                    _epiServerDependencies.ContextResolver.RootPage);

//                if (existingBlogHomePage != null && existingBlogHomePage.Where(x => x.PageName == pageName).Any())
//                    return null;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }

//            var blogHomePage =
//                _epiServerDependencies.ContentRepository.GetDefault<BlogHomePage>
//                (_epiServerDependencies.ContextResolver.RootPage);

//            blogHomePage.Name = pageName;
//            return _epiServerDependencies.ContentRepository.Save(blogHomePage, SaveAction.Publish, AccessLevel.NoAccess);
//        }

//        public void CreateBlogPages(ContentReference blogHomePage)
//        {
//            //var blogPageOne =
//            //    _epiServerDependencies.ContentRepository.GetDefault<BlogPage>
//            //    (blogHomePage);

//            //if ()
            
//            //blogPageOne.Name = "Blog Page 1";

//            //return _epiServerDependencies.ContentRepository.Save(blogPageOne, SaveAction.Publish, AccessLevel.NoAccess);
//        }
//    }
//}