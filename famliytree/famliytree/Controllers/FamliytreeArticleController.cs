using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using famliytree.Models;
using DB.FamliytreeArticle;
using Models.ViewModel;
using System.Web.Http;

namespace famliytree.Controllers
{
    public class FamliytreeArticleController : ApiController
    {
      
        public IEnumerable<FamliyTreeArticleView> GetAllProducts()
        {
            FamliytreeArticle famliytreeArticle = new FamliytreeArticle();
            return famliytreeArticle.GetAll();

        }
        public IEnumerable<FamliyTreeArticleView> GetMessagesByParameter(string title)
        {
            FamliytreeArticle famliytreeArticle = new FamliytreeArticle();
            return famliytreeArticle.GetMessagesByTitle(title);
        }





    }
}