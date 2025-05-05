using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class UserCredentialsController : Controller
    {
        
        // GET: UserCredentials
        private ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult ManageUsers()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            var roles = roleMngr.Roles.Where(x => x.Name != "Administrator" && x.Name != "Student").ToList();
            List<SelectListItem> roleslist = new List<SelectListItem>();
            var allAspNetUsers = _context.Users.ToList();

            if (roles != null)
            {
                foreach (var item in roles)
                {
                    roleslist.Add(new SelectListItem { Text = item.Name, Value = item.Id });
                }
            }

            ViewBag.Roles = roleslist;

            var all = _context.Tbl_UserManagement.ToList();
            List<Tbl_UserManagementViewModel> allData = new List<Tbl_UserManagementViewModel>();
            foreach (var item in all)
            {
                var user = allAspNetUsers.FirstOrDefault(x => x.UserId == item.UserId);
                var role = user != null ? user.Roles.FirstOrDefault() : null;
                var roleId = role != null ? role.RoleId : null;
                var roleText = roleslist.FirstOrDefault(x => x.Value == roleId)?.Text ?? "No Role";

                allData.Add(new Tbl_UserManagementViewModel
                {
                    Description = item.Description,
                    Email = item.Email,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    UserRole = roleText
                });
            }
            return View(allData);
        }

        [HttpPost]
        public ActionResult CreateUser(Tbl_UserManagementViewModel tbl_UserManagementViewModel)
        {
            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement { 
            Description= tbl_UserManagementViewModel.Description,
            Email= tbl_UserManagementViewModel.Email,
            Password= tbl_UserManagementViewModel.Password,
            UserName= tbl_UserManagementViewModel.UserName
            };
            var usermanagement = _context.Tbl_UserManagement.Add(tbl_UserManagement);
            _context.SaveChanges();
            if (usermanagement.UserId>0)
            {
                Random rnd = new Random();
                int rndnumber = rnd.Next(1, 999999);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                PasswordHasher PasswordHash = new PasswordHasher();
                if (UserManager.FindByEmail(tbl_UserManagementViewModel.Email) == null)
                {
                    ApplicationUser admin = new ApplicationUser
                    {
                        UserName = usermanagement.UserName,
                        Email = usermanagement.Email,
                        PasswordHash = PasswordHash.HashPassword(usermanagement.Password),
                        UserId = usermanagement.UserId,
                        PhoneNumber=null,
                        IsEnable=true
                    };

                    IdentityResult result = UserManager.Create(admin);
                    if (result.Succeeded == true)
                    {
                        var  data = UserManager.FindByEmail(admin.Email);
                        if (data != null)
                        {
                            string id = data.Id;
                            UserManager.AddToRole(id, tbl_UserManagementViewModel.UserRole);
                        }
                    }
                   
                }
            }

            return RedirectToAction("ManageUsers");
        }

        public ActionResult RolePermissions()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            List<SelectListItem> roleslist = new List<SelectListItem>();
            var roles = roleMngr.Roles.ToList();
            roleslist.Add(new SelectListItem { Text = "--Select Role--", Value = "0" });
            foreach (var item in roles)
            {
                roleslist.Add(new SelectListItem { Text = item.Name, Value = item.Id });
            }

            ViewBag.Roles = roleslist;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult SaveUpdateRolePermissions(List<RolePagePermission> data)
        {
            if (data.Count()>0)
            {
                //delete all roles
                var rolename = data.FirstOrDefault().RoleName;
                var allrolePermission = _context.RolePagePermissions.Where(x => x.RoleName == rolename).ToList();
                _context.RolePagePermissions.RemoveRange(allrolePermission);
                _context.SaveChanges();
                //delete all entiry for role

                foreach (var item in data)
                    {
                        if (item.PageName != "on")
                        {
                            _context.RolePagePermissions.Add(item);
                        }
                    }
                    _context.SaveChanges();
            }
            return  Json(true);
        }

        public JsonResult GetAllPageByRoleName(string roleName)
        {
            var allrolePermission = _context.RolePagePermissions.Where(x => x.RoleName == roleName).ToList();
            return Json(allrolePermission, JsonRequestBehavior.AllowGet);
        }

        #region RolePermission New Code

        public ActionResult RolePermissionnew()
        {
            var pagename = "Role Permissionsnew";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";
            var data = _context.Tbl_MenuName.ToList();
            ViewBag.MenuList = data;
            List<Tbl_MenuViewmodel> tbl_MenuViewmodels = new List<Tbl_MenuViewmodel>();
            foreach(var item in data)
            {
                tbl_MenuViewmodels.Add(new Tbl_MenuViewmodel
                {
                    MenuName = item.Menu_Name,
                    Menu_Id = item.Menu_Id,
                    Editpermission = CheckEditpermission(pagename, editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)

                });
            }

            ViewBag.MenuList1 = tbl_MenuViewmodels;

            var data1 = _context.Tbl_SubmenuName.ToList();
            List<SubmenuList> Tbl_SubmenuName = new List<SubmenuList>();
            foreach(var item in data1)
            {
                Tbl_SubmenuName.Add(new SubmenuList
                {
                    Submenu_Id = item.Submenu_Id,
                    Menu_Id = item.Menu_Id,
                    MenuName = data.FirstOrDefault(x => x.Menu_Id == item.Menu_Id)?.Menu_Name,
                    Submenu_Name = item.Submenu_Name,
                    Submenu_Url = item.Submenu_Url,
                    Editpermission = CheckEditpermission(pagename,editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)
                });
            }

            ViewBag.SubmenuList = Tbl_SubmenuName;


            var roleStore = new RoleStore<IdentityRole>(_context);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            List<SelectListItem> roleslist = new List<SelectListItem>();
            var roles = roleMngr.Roles.ToList();
            roleslist.Add(new SelectListItem { Text = "--Select Role--", Value = "0" });
            foreach (var item in roles)
            {
                roleslist.Add(new SelectListItem { Text = item.Name, Value = item.Id });
            }

            ViewBag.Roles = roleslist;

            var staffdetails = _context.StafsDetails.ToList();
            ViewBag.StaffDetails = staffdetails;

            


            var createper = CheckCreatepermission(pagename, createpermission);

            ViewBag.Createpermission = createper;


            return View();
        }


        #region CreateMenu
        public ActionResult AddBank(Tbl_MenuName tbl_MenuName)
        {
            try
            {
                _context.Tbl_MenuName.Add(tbl_MenuName);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetMenuById(int Id)
        {
            try
            {
                var data = _context.Tbl_MenuName.FirstOrDefault(x => x.Menu_Id == Id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateMenu(Tbl_MenuName tbl_MenuName)
        {
            try
            {
                var data = _context.Tbl_MenuName.FirstOrDefault(x => x.Menu_Id== tbl_MenuName.Menu_Id);

                if (data != null)
                {
                    _context.Entry(data).CurrentValues.SetValues(tbl_MenuName);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');alert(Data Not Updated);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteMenu(int Id)
        {
            try
            {
                var data = _context.Tbl_MenuName.FirstOrDefault(x => x.Menu_Id == Id);
                if (data != null)
                {
                    _context.Tbl_MenuName.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');alert(Data Not Deleted);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region Create SubMenu
        public ActionResult AddSubmenu(Tbl_SubmenuName tbl_SubmenuName)
        {
            try
            {
                _context.Tbl_SubmenuName.Add(tbl_SubmenuName);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetSubMenuById(int Id)
        {
            try
            {
                var data = _context.Tbl_SubmenuName.FirstOrDefault(x => x.Submenu_Id == Id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateSubMenu(Tbl_SubmenuName tbl_SubmenuName)
        {
            try
            {
                var data = _context.Tbl_SubmenuName.FirstOrDefault(x => x.Submenu_Id == tbl_SubmenuName.Submenu_Id);

                if (data != null)
                {
                    _context.Entry(data).CurrentValues.SetValues(tbl_SubmenuName);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');alert(Data Not Updated);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteSubMenu(int Id)
        {
            try
            {
                var data = _context.Tbl_SubmenuName.FirstOrDefault(x => x.Submenu_Id == Id);
                if (data != null)
                {
                    _context.Tbl_SubmenuName.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/UserCredentials/RolePermissionnew');alert(Data Not Deleted);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        public JsonResult GetSubmenunameById(int id,string roleid,int stafid,string selectedtext)
        {
            try
            {
                var html = "";

                if(roleid != "0" && stafid != 0)
                {
                    var data1 = _context.Tbl_RolePermissionNew.Where(x => x.Role_Id == roleid && x.Staff_Id == stafid && x.Menu_Id == id).ToList();
                    var allsubmenus = _context.Tbl_SubmenuName.Where(x => x.Menu_Id == id).ToList();
                    foreach (var item in allsubmenus)
                    {
                        if (!data1.Any(x => x.Submenu_Id == item.Submenu_Id))
                        {
                            Tbl_RolePermissionNew tbl_RolePermissionNew = new Tbl_RolePermissionNew()
                            {
                                Submenu_Name = item.Submenu_Name,
                                Submenu_Url = item.Submenu_Url,
                                Create_permission = false,
                                Submenu_Id = item.Submenu_Id,
                                Edit_Permission = false,
                                Update_Permission = false,
                                Delete_Permission = false

                            };
                            data1.Add(tbl_RolePermissionNew);
                        }                                                
                    }
                    if(data1.Count > 0)
                    {
                        foreach (var item in data1)
                        {
                            html += "<tr>";
                            html += "<td>" + item.Submenu_Name + "</td>";
                            html += "<td>";
                            html += "<div>";
                            if (item.Submenu_permission == true)
                            {
                                html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission' checked/>";
                            }
                            else
                            {
                                html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission'/>";
                            }
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            if(item.Create_permission == true)
                            {
                                html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission' checked/>";
                            }
                            else
                            {
                                html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission'/>";
                            }
                            
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            if(item.Edit_Permission == true)
                            {
                                html += "<input type='checkbox' class='EditPermission' name2='MenuPermission' checked/>";

                            }
                            else
                            {
                                html += "<input type='checkbox' class='EditPermission' name2='MenuPermission'/>";

                            }
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            if (item.Update_Permission == true)
                            {

                                html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission' checked/>";
                            }
                            else
                            {
                                html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission'/>";

                            }
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            if(item.Delete_Permission == true)
                            {
                                html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission' checked/>";

                            }
                            else
                            {
                                html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission'/>";

                            }
                            html += "</div>";
                            html += "</td>";
                            html += "</tr>";
                        }
                    }
                    else
                    {
                        var data5 = _context.Tbl_SubmenuName.Where(x => x.Menu_Id == id).ToList();
                        if (data5.Count > 0)
                        {
                            foreach (var item in data5)
                            {
                                html += "<tr>";
                                html += "<td>" + item.Submenu_Name + "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' class='EditPermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "</tr>";
                            }
                        }
                    }
                }

                else if(roleid != "0" && selectedtext == "Administrator" || selectedtext == "Student")
                {
                    var data2 = _context.Tbl_RolePermissionNew.Where(x => x.Role_Id == roleid && x.Menu_Id == id).ToList();
                    //if (data2.Count > 0)
                    //{
                    //    foreach (var item in data2)
                    //    {
                    //        html += "<tr>";
                    //        html += "<td>" + item.Submenu_Name + "</td>";
                    //        html += "<td>";
                    //        html += "<div>";
                    //        if(item.Submenu_permission == true)
                    //        {

                    //        html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission' checked/>";
                    //        }
                    //        else
                    //        {
                    //            html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission'/>";

                    //        }
                    //        html += "</div>";
                    //        html += "</td>";
                    //        html += "<td>";
                    //        html += "<div>";
                    //        if(item.Create_permission == true)
                    //        {

                    //        html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission' checked/>";
                    //        }
                    //        else
                    //        {
                    //            html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission'/>";
                    //        }
                    //        html += "</div>";
                    //        html += "</td>";
                    //        html += "<td>";
                    //        html += "<div>";
                    //        if(item.Edit_Permission == true)
                    //        {
                    //        html += "<input type='checkbox' class='EditPermission' name2='MenuPermission' checked/>";
                    //        }
                    //        else
                    //        {
                    //            html += "<input type='checkbox' class='EditPermission' name2='MenuPermission'/>";

                    //        }
                    //        html += "</div>";
                    //        html += "</td>";
                    //        html += "<td>";
                    //        html += "<div>";
                    //        if(item.Update_Permission == true)
                    //        {
                    //        html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission' checked/>";
                    //        }
                    //        else
                    //        {
                    //            html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission'/>";
                    //        }
                    //        html += "</div>";
                    //        html += "</td>";
                    //        html += "<td>";
                    //        html += "<div>";
                    //        if(item.Delete_Permission == true)
                    //        {
                    //        html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission' checked/>";
                    //        }
                    //        else
                    //        {
                    //            html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission'/>";
                    //        }
                    //        html += "</div>";
                    //        html += "</td>";
                    //        html += "</tr>";
                    //    }
                    //}
                    //else
                    {
                        var data6 = _context.Tbl_SubmenuName.Where(x => x.Menu_Id == id).ToList();
                        if (data6.Count > 0)
                        {
                            foreach (var item in data6)
                            {
                                html += "<tr>";
                                html += "<td>" + item.Submenu_Name + "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' class='EditPermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "<td>";
                                html += "<div>";
                                html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission'/>";
                                html += "</div>";
                                html += "</td>";
                                html += "</tr>";
                            }
                        }
                    }
                }
                else if (id != 0)
                {
                    var data = _context.Tbl_SubmenuName.Where(x => x.Menu_Id == id).ToList();
                    if (data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            html += "<tr>";
                            html += "<td>" + item.Submenu_Name + "</td>";
                            html += "<td>";
                            html += "<div>";
                            html += "<input type='checkbox' name='" + item.Submenu_Url + "' class='Submenu_permission' name2='MenuPermission'/>";
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            html += "<input type='checkbox' name='" + item.Submenu_Id + "' name1='" + item.Submenu_Name + "' class='CreatePermission' name2='MenuPermission'/>";
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            html += "<input type='checkbox' class='EditPermission' name2='MenuPermission'/>";
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            html += "<input type='checkbox' class='UpdatePermission' name2='MenuPermission'/>";
                            html += "</div>";
                            html += "</td>";
                            html += "<td>";
                            html += "<div>";
                            html += "<input type='checkbox' class='Deletepermission' name2='MenuPermission'/>";
                            html += "</div>";
                            html += "</td>";
                            html += "</tr>";
                        }
                    }
                }


                if(html != "")
                {

                return Json(html, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult SavemenuPersmission(List<Tbl_RolePermissionNew> data)
        {
            try
            {
                var tblrolepermission = _context.Tbl_RolePermissionNew.ToList();
                if(data.Count > 0)
                {
                    foreach(var item in data)
                    {
                        Tbl_RolePermissionNew roleid = new Tbl_RolePermissionNew();
                        if(item.Staff_Id == 0)
                        {

                        roleid = tblrolepermission.FirstOrDefault(x => x.Role_Id == item.Role_Id && x.Submenu_Id == item.Submenu_Id && x.Menu_Id == item.Menu_Id);
                        }
                        else
                        {
                            roleid = tblrolepermission.FirstOrDefault(x => x.Role_Id == item.Role_Id && x.Submenu_Id == item.Submenu_Id && x.Menu_Id == item.Menu_Id && x.Staff_Id == item.Staff_Id);

                        }
                        if (roleid != null)
                        {
                            item.Rolepermission_Id = roleid.Rolepermission_Id;
                            _context.Entry(roleid).CurrentValues.SetValues(item);
                            _context.SaveChanges();
                        }
                        else
                        {
                            _context.Tbl_RolePermissionNew.Add(item);
                            _context.SaveChanges();
                        }
                      
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Json(true);
        }

        #endregion


        //check tappermission
        //edit permission
        public string CheckEditpermission(string pagename, string permission)
        {
            try
            {
                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Edit_Permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Edit_Permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //create permission
        public string CheckCreatepermission(string pagename, string permission)
        {
            try
            {

                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Create_permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Create_permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete Permission
        public string CheckDeletepermission(string pagename, string permission)
        {
            try
            {
                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Delete_Permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Delete_Permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //View Permission

        public string CheckViewpermission(string pagename, string permission)
        {
            try
            {
                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Update_Permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Update_Permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Register User (Schools)
        public ActionResult RegisterSchools()
        {

            var admindata = _context.Tbl_UserManagement.Where(x => x.Description == "AdminLogin").ToList();

            ViewBag.Admindata = admindata;

            return View();
        }


        public ActionResult AddSchoolLogin(SchoolLoginviewmodel schoolLoginviewmodel)
        {
            try
            {
                var result = "";
                if (schoolLoginviewmodel.Username != null && schoolLoginviewmodel.Password != null)
                {
                    result = Createschhologin(schoolLoginviewmodel);
                }
                if(result != null && result != "")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Added Successfully');location.replace('/UserCredentials/RegisterSchools')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Added');location.replace('/UserCredentials/RegisterSchools')</script>");
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            //return View();
        }

        public JsonResult AddSchoologinAsync(SchoolLoginviewmodel schoolLoginviewmodel)
        {
            try
            {
                //PasswordHasher passwordHasher = new PasswordHasher();
                //schoolLoginviewmodel.Password = passwordHasher.HashPassword(schoolLoginviewmodel.Password);
                var result = "";
                if(schoolLoginviewmodel.Username != null && schoolLoginviewmodel.Password != null)
                {
                 result=  Createschhologin(schoolLoginviewmodel); 
                }

                if (result != null && result != "")
                {
                return Json("Sucess", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Createschhologin(SchoolLoginviewmodel tbl_UserManagementViewModel)
        {
            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement
            {
                Description = "AdminLogin",
                Email = tbl_UserManagementViewModel.Email,
                Password = tbl_UserManagementViewModel.Password,
                UserName = tbl_UserManagementViewModel.Username
            };
            var usermanagement = _context.Tbl_UserManagement.Add(tbl_UserManagement);
            _context.SaveChanges();
            if (usermanagement.UserId > 0)
            {
                Random rnd = new Random();
                int rndnumber = rnd.Next(1, 999999);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                PasswordHasher PasswordHash = new PasswordHasher();
                //if (UserManager.FindByEmail(tbl_UserManagementViewModel.Email) == null)
                //{
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = usermanagement.UserName,
                    Email = usermanagement.Email,
                    PasswordHash = PasswordHash.HashPassword(usermanagement.Password),
                    UserId = usermanagement.UserId,
                    PhoneNumber = tbl_UserManagementViewModel.Phoneno,
                    IsEnable = true
                };

                IdentityResult result = UserManager.Create(admin);
                if (result.Succeeded == true)
                {
                    var data = UserManager.FindByEmail(admin.Email); // Name based user role assignment
                    if (data != null)
                    {
                        string id = data.Id;
                        UserManager.AddToRole(id, "Administrator");
                    }
                }

                // }
            }
            return usermanagement.UserId.ToString();
        }

    }

    public class SubmenuList
    {
        public int Submenu_Id { get; set; }

        public string Submenu_Name { get; set; }

        public int Menu_Id { get; set; }
        public string MenuName { get; set; }
        public string Submenu_Url { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }


    }
}