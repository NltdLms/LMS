﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using NLTD.EmploeePortal.LMS.Dac.DbModel;
using System.Data.Entity.ModelConfiguration;

namespace NLTD.EmploeePortal.LMS.Dac
{
    public class NLTDDbContext : DbContext
    {
        public NLTDDbContext() : base("LmsdbConn")
        {
            return;
        }

      



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer(new Startup().Configuration.GetSection("Data:DefaultConnection:NLTDDbContext").Value.ToString());
        //    optionsBuilder.UseSqlServer("data source=NLTI70;initial catalog=LMS;User ID=sa;Password=password@123;persist security info=False;packet size=4096");
        //}

        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalance { get; set; }
        public DbSet<EmployeeRole> EmployeeRole { get; set; }
        public DbSet<Leave> Leave { get; set; }

        public DbSet<LeaveDetail> LeaveDetail { get; set; }

        public DbSet<PermissionDetail> PermissionDetail { get; set; }
        public DbSet<LeaveAttachment> LeaveAttachment { get; set; }
        public DbSet<LeaveTransactionHistory> LeaveTransactionHistory { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<OfficeLocation> OfficeLocation { get; set; }
        public DbSet<OfficeHoliday> OfficeHoliday { get; set; }

        public DbSet<EmployeeTransactionHistory>EmployeeTransactionHistory { get; set; }
        public DbSet<EmployeeWeekOff> EmployeeWeekOff { get; set; }

        public DbSet<DaysOfWeek> DayOfWeek { get; set; }

        public DbSet<EmployeeAttendance> EmployeeAttendance { get; set; }

        public DbSet<ShiftMaster> ShiftMaster { get; set; }

        public DbSet<ShiftMapping> ShiftMapping { get; set; }
        public DbSet<ShiftTransaction> ShiftTransaction { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.UserId).ToTable("Employee");
            modelBuilder.Entity<EmployeeLeaveBalance>().HasKey(e => e.LeaveBalanceId).ToTable("EmployeeLeaveBalance");
            modelBuilder.Entity<EmployeeRole>().HasKey(e => e.RoleId).ToTable("EmployeeRole");
            modelBuilder.Entity<Leave>().HasKey(e => e.LeaveId).ToTable("Leave");
            modelBuilder.Entity<LeaveDetail>().HasKey(e => e.LeaveDetailId).ToTable("LeaveDetail");
            modelBuilder.Entity<PermissionDetail>().HasKey(e => e.PermissionDetailId).ToTable("PermissionDetail");
            modelBuilder.Entity<LeaveAttachment>().HasKey(e => e.AttachmentId).ToTable("LeaveAttachment");
            modelBuilder.Entity<LeaveType>().HasKey(e => e.LeaveTypeId).ToTable("LeaveType");
            modelBuilder.Entity<LeaveTransactionHistory>().HasKey(e => e.TransactionId).ToTable("LeaveTransactionHistory");
            modelBuilder.Entity<EmployeeTransactionHistory>().HasKey(e => e.TransactionId).ToTable("EmployeeTransactionHistory");
            modelBuilder.Entity<LeaveType>().HasKey(e => e.LeaveTypeId).ToTable("LeaveType");
            modelBuilder.Entity<OfficeLocation>().HasKey(e => e.OfficeId).ToTable("OfficeLocation");
            modelBuilder.Entity<OfficeHoliday>().HasKey(e => e.OfficeHolodayId).ToTable("OfficeHoliday");
            
            modelBuilder.Entity<EmployeeWeekOff>().HasKey(e => e.EmployeeWeekOffId).ToTable("EmployeeWeekOff");
            modelBuilder.Entity<DaysOfWeek>().HasKey(e => e.DaysOfWeekId).ToTable("DaysOfWeek");
            modelBuilder.Entity<EmployeeAttendance>().HasKey(e => e.ID).ToTable("EmployeeAttendance");
            modelBuilder.Entity<ShiftMaster>().HasKey(e => e.ShiftID).ToTable("ShiftMaster");
            modelBuilder.Entity<ShiftMapping>().HasKey(e => e.ShiftMappingID).ToTable("ShiftMapping");
            modelBuilder.Entity<ShiftTransaction>().HasKey(e => e.ShiftTransactionID).ToTable("ShiftTransaction");

        }
    }
}
