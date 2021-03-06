using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRN211_Project_OBS.Models;

namespace PRN211_Project_OBS.Controllers
{
    public class HomeController : Controller
    {
        DAO dao = new DAO();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListNewBook = dao.GetNewestBook();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            ViewBag.Url = "/Home/Index";
            return View();
        }

        [HttpGet]
        public ActionResult Genre()
        {
            string genre_id = Request.Params["genre_id"];
            List<Book> list = dao.GetBookByGenre(genre_id);
            int pageSize = list.Count % 6 == 0 ? list.Count / 6 : list.Count / 6 + 1;
            int currentPage;
            try
            {
                currentPage = Int32.Parse(Request.Params["page"]);
            }
            catch (Exception e)
            {
                currentPage = 1;
            }
            Genre genre = dao.GetGenreById(genre_id);
            ViewBag.ListBook = list.GetRange(6 * (currentPage - 1), 6 * currentPage > list.Count ? list.Count % 6 : 6);
            ViewBag.Genre = genre;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = currentPage;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            return View();
        }

        [HttpGet]
        public ActionResult BookDetail()
        {
            string id = Request.Params["book_id"];
            ViewBag.Param_Id = id;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.Book = dao.GetBookById(id);
            ViewBag.Author = dao.GetAuthorById(dao.GetBookById(id).author_id.ToString());
            ViewBag.GenreBook = dao.GetGenreByBookId(id);
            ViewBag.Relate = dao.GetRelatedBook(dao.GetBookById(id));
            return View();
        }

        [HttpGet]
        public ActionResult Filter()
        {
            string[] checkCate = Request.Params.GetValues("checkCate");
            List<Book> list = new List<Book>();
            string tagnav = "";
            string url_part = "";
            string tag = "";
            if (checkCate == null)
            {
                tagnav = "no filter";
            }
            else
            {
                for (int i = 0; i < checkCate.Length; i++)
                {
                    url_part += $"checkCate={checkCate[i]}&";
                    if (i == checkCate.Length - 1) tagnav += dao.GetGenreById(checkCate[i]).name;
                    else tagnav += dao.GetGenreById(checkCate[i]).name + ", ";
                }
                for (int i = 0; i < checkCate.Length; i++)
                {
                    tag += checkCate[i];
                }
            }
            list = dao.getBookByFilter(checkCate);
            int pageSize = list.Count % 6 == 0 ? list.Count / 6 : list.Count / 6 + 1;
            int currentPage;
            try
            {
                currentPage = Int32.Parse(Request.Params["page"]);
            }
            catch (Exception)
            {
                currentPage = 1;
            }
            ViewBag.ListBook = list.GetRange(6 * (currentPage - 1), 6 * currentPage > list.Count ? list.Count % 6 : 6);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Tag = tag;
            ViewBag.TagNav = tagnav;
            ViewBag.Url_Part = url_part;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            return View();
        }

        [HttpPost]
        public ActionResult Filter(string[] checkCate)
        {
            List<Book> list = new List<Book>();
            string tagnav = "";
            string url_part = "";
            string tag = "";
            if (checkCate == null)
            {
                tagnav = "no filter";
            } 
            else
            {
                for (int i = 0; i < checkCate.Length; i++)
                {
                    url_part += $"checkCate={checkCate[i]}&";
                    if (i == checkCate.Length - 1) tagnav += dao.GetGenreById(checkCate[i]).name;
                    else tagnav += dao.GetGenreById(checkCate[i]).name + ", ";
                }
                for (int i = 0; i < checkCate.Length; i++)
                {
                    tag += checkCate[i];
                }
            }
            list = dao.getBookByFilter(checkCate);
            int pageSize = list.Count % 6 == 0 ? list.Count / 6 : list.Count / 6 + 1;
            int currentPage;
            try
            {
                currentPage = Int32.Parse(Request.Params["page"]);
            }
            catch (Exception e)
            {
                currentPage = 1;
            }
            ViewBag.ListBook = list.GetRange(6 * (currentPage - 1), 6 * currentPage > list.Count ? list.Count % 6 : 6);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Tag = tag;
            ViewBag.TagNav = tagnav;
            ViewBag.Url_Part = url_part;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            return View();
        }

        [HttpGet]
        public ActionResult BookOfAuthor()
        {
            int id = Convert.ToInt32(Request.Params["author_id"]);
            List<Book> list = dao.GetBooksByAuthor(id);
            int pageSize = list.Count % 6 == 0 ? list.Count / 6 : list.Count / 6 + 1;
            int currentPage;
            try
            {
                currentPage = Int32.Parse(Request.Params["page"]);
            }
            catch (Exception e)
            {
                currentPage = 1;
            }
            ViewBag.Tag = dao.GetAuthorById(id.ToString()).name;
            ViewBag.id = id;
            ViewBag.ListBook = list.GetRange(6 * (currentPage - 1), 6 * currentPage > list.Count ? list.Count % 6 : 6);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = currentPage;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchContent)
        {
            List<Book> slist = dao.GetBooksByName(searchContent);
            int pageSize = slist.Count % 6 == 0 ? slist.Count / 6 : slist.Count / 6 + 1;
            int currentPage;
            try
            {
                currentPage = Int32.Parse(Request.Params["page"]);
            }
            catch (Exception e)
            {
                currentPage = 1;
            }
            ViewBag.ListBook = slist.GetRange(6 * (currentPage - 1), 6 * currentPage > slist.Count ? slist.Count % 6 : 6);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = currentPage;
            ViewBag.sc = searchContent;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            string searchContent = Request.Params["searchContent"];
            List<Book> slist = dao.GetBooksByName(searchContent);
            int pageSize = slist.Count % 6 == 0 ? slist.Count / 6 : slist.Count / 6 + 1;
            int currentPage;
            try
            {
                currentPage = Int32.Parse(Request.Params["page"]);
            }
            catch (Exception e)
            {
                currentPage = 1;
            }
            ViewBag.ListBook = slist.GetRange(6 * (currentPage - 1), 6 * currentPage > slist.Count ? slist.Count % 6 : 6);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = currentPage;
            ViewBag.sc = searchContent;
            ViewBag.ListGenre = dao.GetGenres();
            ViewBag.ListBestSeller = dao.GetBestSellerBook();
            return View();
        }
    }
}