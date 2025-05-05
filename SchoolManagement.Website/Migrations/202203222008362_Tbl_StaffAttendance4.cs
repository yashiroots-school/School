namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffAttendance4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StaffAttendance", "Dayone", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwo", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daythree", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayfour", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayfive", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daysix", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayseven", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayeight", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daynine", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayten", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayeleven", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwelve", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daythirteen", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayfifteen", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daysisteen", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayseventeen", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayeighten", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daynineten", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Dayotwenty", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentyone", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentytwo", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentythree", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentyfour", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentyfive", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentysix", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentyseven", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentyeight", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daytwentynine", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daythirty", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Daythirtyone", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Total", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StaffAttendance", "Total");
            DropColumn("dbo.Tbl_StaffAttendance", "Daythirtyone");
            DropColumn("dbo.Tbl_StaffAttendance", "Daythirty");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentynine");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentyeight");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentyseven");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentysix");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentyfive");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentyfour");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentythree");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentytwo");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwentyone");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayotwenty");
            DropColumn("dbo.Tbl_StaffAttendance", "Daynineten");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayeighten");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayseventeen");
            DropColumn("dbo.Tbl_StaffAttendance", "Daysisteen");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayfifteen");
            DropColumn("dbo.Tbl_StaffAttendance", "Daythirteen");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwelve");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayeleven");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayten");
            DropColumn("dbo.Tbl_StaffAttendance", "Daynine");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayeight");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayseven");
            DropColumn("dbo.Tbl_StaffAttendance", "Daysix");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayfive");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayfour");
            DropColumn("dbo.Tbl_StaffAttendance", "Daythree");
            DropColumn("dbo.Tbl_StaffAttendance", "Daytwo");
            DropColumn("dbo.Tbl_StaffAttendance", "Dayone");
        }
    }
}
