using SchoolManagement.Website.EnumData;
using SchoolManagement.Website.Models;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class AppConfigurationController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: AppConfiguration
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult BindLeftMenuData()
        {
            var loginUserRoleID = Session["LoginRoleID"].ToString();
            var rolePermissionList = _context.RolePagePermissions.Where(x => x.RoleId == loginUserRoleID).ToList();
            StringBuilder menuHTML = new StringBuilder();
            foreach (int item in Enum.GetValues(typeof(SideParentMenuEnum)))
            {
                string htmlSub = string.Empty;
                string li = string.Empty;
                var RootName = Enum.GetName(typeof(SideParentMenuEnum), item);
                var rolePermissionListByRootId = rolePermissionList.Where(x => x.ParentId == item);
                if (rolePermissionListByRootId.ToList().Count > 0)
                {
                    htmlSub = @"<li>
                                    <a><i class='fa fa-arrow-circle-o-down'></i>" + RootName + " <span class='fa fa-chevron-down'></span></a>" +
                                       "<ul class='nav child_menu' style='display: none'>{0}</ul></li>";

                    foreach (var pageName in rolePermissionListByRootId)
                    {
                        li += "<li><a href='" + pageName.PageName + "'>" + pageName.PageViewName + "</a></li>";
                    }
                    var html = string.Format(htmlSub, li);
                    menuHTML.Append(html);
                }

            }

            return Json(menuHTML.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}