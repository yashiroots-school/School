using EmployeeManagement.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SchoolManagement.Data.Constants;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SchoolManagement.Website
{
    public partial class Startup
    {
        private IRepository<Accounts> _accountsRepository = null;
        private IRepository<Frequencys> _frequencyRepository = null;
        private IRepository<FeeHeadingGroups> _feeHeadingGroupsRepository = null;
        private IRepository<Classes> _ClassesRepository = null;
        private IRepository<StudentCategorys> _StudentCategorysRepository = null;



        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            _accountsRepository = new Repository<Accounts>();
            _frequencyRepository = new Repository<Frequencys>();
            _feeHeadingGroupsRepository = new Repository<FeeHeadingGroups>();
            _ClassesRepository = new Repository<Classes>();
            _StudentCategorysRepository = new Repository<StudentCategorys>();

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(60),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                //CookieHttpOnly = true,
                //SlidingExpiration=true,
                //CookieManager = new SystemWebCookieManager()

            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
            ConfigureJsonSerialization();


            CreateRoles();
            CreateUser();
            //CreateFeeAccounts();
            CreateFrequency();
            //CreateFeeGroup();
            CreateClasses();
            CreateStudentCategory();
        }

        public void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (roleManager != null)
            {
                if (!roleManager.RoleExists("Administrator"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Administrator"
                    };
                    roleManager.Create(role);

                }
                if (!roleManager.RoleExists("Student"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Student"
                    };
                    roleManager.Create(role);
                }
                if (!roleManager.RoleExists("Staff"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Staff"
                    };
                    roleManager.Create(role);
                }
                if (!roleManager.RoleExists("Receptionist"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Receptionist"
                    };
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Account"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Account"
                    };
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Library"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Library"
                    };
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Developer"))
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Developer"
                    };
                    roleManager.Create(role);
                }
            }


        }
        public void CreateUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            PasswordHasher PasswordHash = new PasswordHasher();
            //if (UserManager.FindByEmail("johnmathewcssr79@gmail.com") == null)
            //{
            //    ApplicationUser admin = new ApplicationUser
            //    {
            //        UserName = "admin",
            //        Email = "johnmathewcssr79@gmail.com",
            //        PhoneNumber = "7411109976",
            //        PasswordHash = PasswordHash.HashPassword("StAlphonsa")
            //    };

            //    IdentityResult result = UserManager.Create(admin);
            //    string Id = UserManager.FindByEmail(admin.Email).Id;
            //    UserManager.AddToRole(Id, "Administrator");
            //}
            if (UserManager.FindByEmail("iroots@adminmail.com") == null)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = "iroots",
                    Email = "iroots@adminmail.com",
                    PhoneNumber = "9407023075",
                    PasswordHash = PasswordHash.HashPassword("Lumen@123")
                };

                IdentityResult result = UserManager.Create(admin);
                string Id = UserManager.FindByEmail(admin.Email).Id;
                UserManager.AddToRole(Id, "Developer");
            }
            //if (UserManager.FindByEmail("iroots@adminmail.com") == null)
            //{
            //    ApplicationUser admin = new ApplicationUser
            //    {
            //        UserName = "sysAdmin",
            //        Email = "iroots@adminmail.com",
            //        PhoneNumber = "9407023075",
            //        PasswordHash = PasswordHash.HashPassword("admin@123")
            //    };

            //    IdentityResult result = UserManager.Create(admin);
            //    string Id = UserManager.FindByEmail(admin.Email).Id;
            //    UserManager.AddToRole(Id, "Administrator");
            //}

            if (UserManager.FindByEmail("default_student@testmail.com") == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "student",
                    Email = "default_student@testmail.com",
                    PasswordHash = PasswordHash.HashPassword("student")
                };

                UserManager.Create(user);
                UserManager.AddToRole(user.Id, "Student");
            }

        }

        public void CreateFeeAccounts()
        {
            System.Collections.Generic.IEnumerable<Accounts> account = _accountsRepository.GetAll();
            foreach (string item in AccountsConst.accountNames)
            {
                if (account.FirstOrDefault(x => x.AccountName == item) == null)
                {
                    Accounts accounts = new Accounts()
                    {
                        AccountName = item
                    };
                    _accountsRepository.Insert(accounts);
                    _accountsRepository.Save();
                }
            }
        }

        public void CreateFrequency()
        {
            var Frequency = _frequencyRepository.GetAll();
            foreach (string item in FrequencyConst.FrequencyList)
            {
                if (Frequency.FirstOrDefault(x => x.FeeFrequencyName == item) == null)
                {
                    Frequencys frequencys = new Frequencys()
                    {
                        FeeFrequencyName = item
                    };
                    _frequencyRepository.Insert(frequencys);
                    _frequencyRepository.Save();
                }
            }
        }

        public void CreateFeeGroup()
        {
            var feeHeadingGroups = _feeHeadingGroupsRepository.GetAll();

            if (feeHeadingGroups.FirstOrDefault(x => x.FeeHeadingGroupName == FeeGroup.FirstGroup) == null)
            {
                FeeHeadingGroups frequencys = new FeeHeadingGroups()
                {
                    FeeHeadingGroupName = FeeGroup.FirstGroup
                };
                _feeHeadingGroupsRepository.Insert(frequencys);
                _feeHeadingGroupsRepository.Save();
            }
        }
        public void CreateClasses()
        {
            var ClassList = _ClassesRepository.GetAll();

            foreach (string item in ClassesConst.ClassesList)
            {
                if (ClassList.FirstOrDefault(x => x.ClassName == item) == null)
                {
                    Classes cls = new Classes()
                    {
                        ClassName = item
                    };
                    _ClassesRepository.Insert(cls);
                    _ClassesRepository.Save();
                }
            }
        }
        public void CreateStudentCategory()
        {
            var categoryList = _StudentCategorysRepository.GetAll();

            foreach (string item in CategoriesConst.catgegoryList)
            {
                if (categoryList.FirstOrDefault(x => x.CategoryName == item) == null)
                {
                    StudentCategorys cls = new StudentCategorys()
                    {
                        CategoryName = item
                    };
                    _StudentCategorysRepository.Insert(cls);
                    _StudentCategorysRepository.Save();
                }
            }
        }
        private void ConfigureJsonSerialization()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }


    }
}