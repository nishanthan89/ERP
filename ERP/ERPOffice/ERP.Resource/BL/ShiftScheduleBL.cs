using ERP.DA;
using ERP.Resource.Models;
using ERP.Resource.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Resource.BL
{
    public class ShiftScheduleBL
    {
        private ERPEntities _db = new ERPEntities();
        /// <summary>
        /// Assign ShiftSchedule
        /// </summary>
        /// <param name="shiftScheduleBO"></param>
        public bool AssignShiftSchedule(ShiftScheduleViewModel shiftScheduleBO, out string msg)
        {
            msg = "";
            int datediffrent = shiftScheduleBO.ShiftEndDate.Date.Subtract(shiftScheduleBO.ShiftStartDate.Date).Days;
            datediffrent = datediffrent + 1;
            int numofDays = shiftScheduleBO.NoofDays;
            DateTime startDate = (DateTime)shiftScheduleBO.ShiftStartDate;

            string assigntype = shiftScheduleBO.AssignType;

            if (assigntype == "Weekly")
            {
                for (int i = 0; i < datediffrent; i++)
                {
                    //string daynames = startDate.DayOfWeek.ToString();

                    if (startDate.DayOfWeek == DayOfWeek.Monday && shiftScheduleBO.WeeklyMonday == true)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);

                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Tuesday && shiftScheduleBO.WeeklyTuesday == true)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Wednesday && shiftScheduleBO.WeeklyWednesday == true)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Thursday && shiftScheduleBO.WeeklyThursday == true)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Friday && shiftScheduleBO.WeeklyFriday == true)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Saturday && shiftScheduleBO.WeeklySaturday == true)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                    }
                    else
                    {
                        if (startDate.DayOfWeek == DayOfWeek.Sunday && shiftScheduleBO.WeeklySunday == true)
                        {
                            shiftScheduleBO.ShiftStartDate = startDate;
                            shiftScheduleBO.ShiftEndDate = startDate;
                            CreateShiftSchedule(shiftScheduleBO, out msg);
                        }
                    }
                    startDate = startDate.AddDays(1);
                }
            }

            else
            {
                if (datediffrent >= numofDays)
                {
                    for (int i = 0; i < numofDays; i++)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                        startDate = startDate.AddDays(1);
                    }
                    startDate = startDate.AddDays(numofDays);
                    datediffrent = datediffrent - 2 * numofDays;

                }
                else
                {
                    for (int i = 0; i < datediffrent; i++)
                    {
                        shiftScheduleBO.ShiftStartDate = startDate;
                        shiftScheduleBO.ShiftEndDate = startDate;
                        CreateShiftSchedule(shiftScheduleBO, out msg);
                        startDate = startDate.AddDays(1);
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Create ShiftSchedule
        /// </summary>
        /// <param name="shiftSchedule"></param>
        /// <returns></returns>
        public bool CreateShiftSchedule(ShiftScheduleViewModel shiftSchedule, out string msg)
        {
            msg = "";
            Resource_ShiftSchedule resource_ShiftSchedule = new Resource_ShiftSchedule();
            var resList = shiftSchedule.ResourceList.Where(x => x.ResourceCheck == true).ToList();
            if (resList.Count != 0)
            {
                //foreach (var x in shiftSchedule.ResourceID)

                resList.ForEach(x =>
                {
                    if (shiftSchedule.YesNo == "No")
                    {
                        // var checkShift = _db.Resource_ShiftSchedule.Where(m => m.EmployeeID == x.ResourceID && m.ShiftDate == shiftSchedule.ShiftStartDate).Select(m => m.ShiftPatternID).ToList();
                        // var empID = int.Parse(x.ResourceID);
                        var check = _db.Resource_ShiftSchedule.Where(m => m.EmployeeID == x.ResourceID && m.ShiftDate == shiftSchedule.ShiftStartDate).FirstOrDefault();

                        resource_ShiftSchedule.EmployeeID = x.ResourceID;
                        resource_ShiftSchedule.ShiftDate = shiftSchedule.ShiftStartDate;
                        resource_ShiftSchedule.ExpectedOnTime = shiftSchedule.ShiftStartDate.Add(shiftSchedule.ExpectedOnTime);
                        resource_ShiftSchedule.ExpectedOffTime = shiftSchedule.ShiftEndDate.Add(shiftSchedule.ExpectedOffTime);
                        resource_ShiftSchedule.ShiftPatternID = shiftSchedule.ShiftPatternID;
                        resource_ShiftSchedule.BranchID = shiftSchedule.BranchNameID;
                        if (shiftSchedule.ActualOnTime != null)
                        {
                            resource_ShiftSchedule.ActualOnTime = shiftSchedule.ShiftStartDate.Add((TimeSpan)shiftSchedule.ActualOnTime);
                        }
                        if (shiftSchedule.ActualOffTime != null)
                        {
                            resource_ShiftSchedule.ActualOffTime = shiftSchedule.ShiftEndDate.Add((TimeSpan)shiftSchedule.ActualOffTime);
                        }
                        _db.Resource_ShiftSchedule.Add(resource_ShiftSchedule);
                        _db.SaveChanges();
                        //}
                    }
                    if (shiftSchedule.YesNo == "Yes")
                    {
                        // var emID = int.Parse(x);
                        var checkYes = (from rss in _db.Resource_ShiftSchedule where rss.EmployeeID == x.ResourceID && rss.ShiftDate == shiftSchedule.ShiftStartDate select rss).ToList();
                        //var checkYes = _db.Resource_ShiftSchedule.Where(m => m.EmployeeID == int.Parse(x) && m.ShiftDate == shiftSchedule.ShiftStartDate).ToList();
                        if (checkYes.Count != 0)
                        {
                            //foreach (var p in checkYes)
                            //{
                            checkYes.ForEach(p =>
                            {
                                Resource_ShiftSchedule shift = _db.Resource_ShiftSchedule.Where(m => m.ShiftScheduleID == p.ShiftScheduleID).FirstOrDefault();
                                if (shift != null)
                                {
                                    _db.Resource_ShiftSchedule.Remove(shift);
                                    _db.SaveChanges();
                                    resource_ShiftSchedule.EmployeeID = x.ResourceID;
                                    resource_ShiftSchedule.ShiftDate = shiftSchedule.ShiftStartDate;
                                    resource_ShiftSchedule.ExpectedOnTime = shiftSchedule.ShiftStartDate.Add(shiftSchedule.ExpectedOnTime);
                                    resource_ShiftSchedule.ExpectedOffTime = shiftSchedule.ShiftEndDate.Add(shiftSchedule.ExpectedOffTime);
                                    resource_ShiftSchedule.ShiftPatternID = shiftSchedule.ShiftPatternID;
                                    resource_ShiftSchedule.BranchID = shiftSchedule.BranchNameID;
                                    if (shiftSchedule.ActualOnTime != null)
                                    {
                                        resource_ShiftSchedule.ActualOnTime = shiftSchedule.ShiftStartDate.Add((TimeSpan)shiftSchedule.ActualOnTime);
                                    }
                                    if (shiftSchedule.ActualOffTime != null)
                                    {
                                        resource_ShiftSchedule.ActualOffTime = shiftSchedule.ShiftEndDate.Add((TimeSpan)shiftSchedule.ActualOffTime);
                                    }
                                    _db.Resource_ShiftSchedule.Add(resource_ShiftSchedule);
                                    _db.SaveChanges();
                                }
                            });
                        }
                        else
                        {
                            resource_ShiftSchedule.EmployeeID = x.ResourceID;
                            resource_ShiftSchedule.ShiftDate = shiftSchedule.ShiftStartDate;
                            resource_ShiftSchedule.ExpectedOnTime = shiftSchedule.ShiftStartDate.Add(shiftSchedule.ExpectedOnTime);
                            resource_ShiftSchedule.ExpectedOffTime = shiftSchedule.ShiftEndDate.Add(shiftSchedule.ExpectedOffTime);
                            resource_ShiftSchedule.ShiftPatternID = shiftSchedule.ShiftPatternID;
                            resource_ShiftSchedule.BranchID = shiftSchedule.BranchNameID;
                            if (shiftSchedule.ActualOnTime != null)
                            {
                                resource_ShiftSchedule.ActualOnTime = shiftSchedule.ShiftStartDate.Add((TimeSpan)shiftSchedule.ActualOnTime);
                            }
                            if (shiftSchedule.ActualOffTime != null)
                            {
                                resource_ShiftSchedule.ActualOffTime = shiftSchedule.ShiftEndDate.Add((TimeSpan)shiftSchedule.ActualOffTime);
                            }
                            _db.Resource_ShiftSchedule.Add(resource_ShiftSchedule);
                            _db.SaveChanges();
                        }
                    }
                });
                msg = "Successfully Created";
                return true;
            }
            msg = "Something Wrong";
            return false;
        }
        /// <summary>
        /// Get Pattern List
        /// </summary>
        /// <returns></returns>
        public List<Resource_ShiftPattern> GetPatternList()
        {
            return _db.Resource_ShiftPattern.ToList();
        }
        /// <summary>
        /// Get Employee List
        /// </summary>
        /// <returns></returns>
        public List<Resource_Employee> GetEmployeeList()
        {
            return _db.Resource_Employee.ToList();
        }
        /// <summary>
        /// Get Resource List
        /// </summary>
        /// <returns></returns>
        public List<Person> GetResourceList()
        {
            var resourceList = (from re in _db.Resource_Employee
                                select new Person
                                {
                                    Resource = re.FirstName,
                                    ResourceID = re.EmployeeID
                                }).OrderBy(i => i.Resource);
            return resourceList.ToList();
        }
        /// <summary>
        /// Get Branch Employee List
        /// </summary>
        /// <param name="branchID"></param>
        /// <returns></returns>
        public List<Person> GetBranchEmployeeList(int branchID)
        {
            var resourceList = (from re in _db.Resource_Employee
                                where re.Resource_BranchDetails.Select(x => x.BranchID == branchID).FirstOrDefault()
                                select new Person
                                {
                                    Resource = re.FirstName,
                                    ResourceID = re.EmployeeID
                                }).OrderBy(i => i.Resource);
            return resourceList.ToList();
        }
        /// <summary>
        /// Get Branch List
        /// </summary>
        /// <returns></returns>
        public List<Branch_Details> GetBranchList()
        {
            return _db.Branch_Details.ToList();
        }
        /// <summary>
        /// Get Shift Pattern
        /// </summary>
        /// <param name="shiftPattrenId"></param>
        /// <returns></returns>
        public Resource_ShiftPattern GetShiftPattern(int shiftPattrenId)
        {
            return _db.Resource_ShiftPattern.Where(x => x.ShiftPatternID == shiftPattrenId).FirstOrDefault();
        }
        /// <summary>
        /// Get ShiftSchedule List
        /// </summary>
        /// <param name="shiftSearch"></param>
        /// <returns></returns>
        public List<ShiftScheduleViewModel> GetShiftScheduleList(ShiftSearchViewModel shiftSearch)
        {
            var shiftList = (from rss in _db.Resource_ShiftSchedule
                             select new ShiftScheduleViewModel

                             {
                                 ShiftScheduleID = rss.ShiftScheduleID,
                                 EmployeeID = (int)rss.EmployeeID,
                                 Employee = rss.Resource_Employee.FirstName,
                                 ShiftStartDate = (DateTime)rss.ShiftDate,
                                 ShiftPatternID = (int)rss.ShiftPatternID,
                                 ShiftPattern = rss.Resource_ShiftPattern.Description,
                                 ExpectedOnDateTime = (DateTime)rss.ExpectedOnTime,
                                 ExpectedOffDateTime = (DateTime)rss.ExpectedOffTime,
                                 ActualOnDateTime = (DateTime)rss.ActualOnTime,
                                 ActualOffDateTime = (DateTime)rss.ActualOffTime,
                                 BranchNameID = (int)rss.BranchID,
                                 BranchName = rss.Branch_Details.BranchName,
                             });
            if (shiftSearch.EmployeeID == 0)
            {
                shiftSearch.EmployeeID = null;
            }
            if (shiftSearch.BranchID == 0)
            {
                shiftSearch.BranchID = null;
            }
            if (shiftSearch.ShiftPatternNameID == 0)
            {
                shiftSearch.ShiftPatternNameID = null;
            }

            if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.BranchNameID == shiftSearch.BranchID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData==0?20:shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.BranchNameID == shiftSearch.BranchID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.BranchNameID == shiftSearch.BranchID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);

            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID && x.BranchNameID == shiftSearch.BranchID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null && shiftSearch.SelectView != "Weekly")
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).Take(shiftSearch.CountData == 0 ? 20 : shiftSearch.CountData);
            }
            return shiftList.ToList();
        }

        public List<ShiftScheduleViewModel> InfinateShiftScheduleList(int BlockNumber, int BlockSize, ShiftSearchViewModel shiftSearch)
        {
            shiftSearch.StartDate = shiftSearch.StartDate != null ? shiftSearch.StartDate : null;
            shiftSearch.EndDate = shiftSearch.EndDate != null ? shiftSearch.EndDate : shiftSearch.EndDate = null;

            int startIndex = (BlockNumber - 1) * BlockSize;
            var shiftList = (from rss in _db.Resource_ShiftSchedule
                             select new ShiftScheduleViewModel

                             {
                                 ShiftScheduleID = rss.ShiftScheduleID,
                                 EmployeeID = (int)rss.EmployeeID,
                                 Employee = rss.Resource_Employee.FirstName,
                                 ShiftStartDate = (DateTime)rss.ShiftDate,
                                 ShiftPatternID = (int)rss.ShiftPatternID,
                                 ShiftPattern = rss.Resource_ShiftPattern.Description,
                                 ExpectedOnDateTime = (DateTime)rss.ExpectedOnTime,
                                 ExpectedOffDateTime = (DateTime)rss.ExpectedOffTime,
                                 ActualOnDateTime = (DateTime)rss.ActualOnTime,
                                 ActualOffDateTime = (DateTime)rss.ActualOffTime,
                                 BranchNameID = (int)rss.BranchID,
                                 BranchName = rss.Branch_Details.BranchName,

                             });
            if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID && x.BranchNameID == shiftSearch.BranchID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.BranchNameID == shiftSearch.BranchID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID == null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.BranchNameID == shiftSearch.BranchID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate != null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= shiftSearch.EndDate && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);

            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.BranchNameID == shiftSearch.BranchID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate == null && shiftSearch.EndDate == null && shiftSearch.EmployeeID != null && shiftSearch.BranchID != null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.EmployeeID == shiftSearch.EmployeeID && x.BranchNameID == shiftSearch.BranchID && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            else if (shiftSearch.StartDate != null && shiftSearch.EndDate == null && shiftSearch.EmployeeID == null && shiftSearch.BranchID == null && shiftSearch.ShiftPatternNameID != null)
            {
                shiftList = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftPatternID == shiftSearch.ShiftPatternNameID).OrderBy(x => x.ShiftStartDate).Skip(startIndex).Take(BlockSize);
            }
            return shiftList.ToList();
        }
        /// <summary>
        /// Delete ShiftSchedule
        /// </summary>
        /// <param name="shiftSearch"></param>
        /// <returns></returns>
        public bool DeleteShiftSchedule(ShiftSearchViewModel shiftSearch, out string msg)
        {
            msg = "";
            try
            {
                var delShift = _db.Resource_ShiftSchedule.Where(x => x.ShiftScheduleID == shiftSearch.ShiftScheduleID).FirstOrDefault();
                if (delShift != null)
                {
                    _db.Resource_ShiftSchedule.Remove(delShift);
                    _db.SaveChanges();
                    msg = "Successfully Removed";
                    return true;
                }
                else
                {
                    msg = "Something Wrong";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

        }
        /// <summary>
        /// Get Edit Schedule
        /// </summary>
        /// <param name="shiftSchedule"></param>
        /// <returns></returns>
        public ShiftScheduleViewModel GetEditSchedule(ShiftScheduleViewModel shiftSchedule, out string msg)
        {
            msg = "";
            var editShift = _db.Resource_ShiftSchedule.Where(x => x.ShiftScheduleID == shiftSchedule.ShiftScheduleID).FirstOrDefault();
            if (editShift != null)
            {
                shiftSchedule.EditResourceID = (int)editShift.EmployeeID;
                shiftSchedule.Employee = _db.Resource_Employee.Where(x => x.EmployeeID == shiftSchedule.EditResourceID).Select(x => x.FirstName).FirstOrDefault();
                shiftSchedule.ShiftPatternID = (int)editShift.ShiftPatternID;
                shiftSchedule.ShiftPattern = _db.Resource_ShiftPattern.Where(x => x.ShiftPatternID == shiftSchedule.ShiftPatternID).Select(x => x.Description).FirstOrDefault();
                shiftSchedule.ShiftStartDate = (DateTime)editShift.ShiftDate;
                shiftSchedule.ExpectedOffTime = (TimeSpan)editShift.ExpectedOffTime.TimeOfDay;
                shiftSchedule.ExpectedOffDateTime = (DateTime)editShift.ShiftDate.Add(editShift.ExpectedOffTime.TimeOfDay);
                shiftSchedule.ExpectedOnTime = (TimeSpan)editShift.ExpectedOnTime.TimeOfDay;
                shiftSchedule.ExpectedOnDateTime = (DateTime)editShift.ShiftDate.Add(editShift.ExpectedOnTime.TimeOfDay);
                if (editShift.ActualOffTime != null)
                {
                    shiftSchedule.ActualOffDateTime = editShift.ActualOffTime;
                    shiftSchedule.ActualOffTime = editShift.ActualOffTime.Value.TimeOfDay;
                }
                if (editShift.ActualOnTime != null)
                {
                    shiftSchedule.ActualOnDateTime = editShift.ActualOnTime;
                    shiftSchedule.ActualOnTime = editShift.ActualOnTime.Value.TimeOfDay;
                }
                shiftSchedule.BranchNameID = (int)editShift.BranchID;
                shiftSchedule.BranchName = _db.Branch_Details.Where(x => x.BranchID == shiftSchedule.BranchNameID).Select(x => x.BranchName).FirstOrDefault();
                return shiftSchedule;
            }
            msg = "Something Wrong";
            return null;
        }
        /// <summary>
        /// Update Schedule
        /// </summary>
        /// <param name="shiftBO"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateSchedule(ShiftBO shiftBO, out string msg)
        {
            try
            {
                msg = "";
                var updateShift = _db.Resource_ShiftSchedule.Where(x => x.ShiftScheduleID == shiftBO.ShiftSchedule.ShiftScheduleID).FirstOrDefault();
                if (updateShift != null)
                {
                    updateShift.EmployeeID = shiftBO.ShiftSchedule.EditResourceID;
                    updateShift.BranchID = shiftBO.ShiftSchedule.BranchNameID;
                    updateShift.ShiftPatternID = shiftBO.ShiftSchedule.ShiftPatternID;
                    updateShift.ShiftDate = shiftBO.ShiftSchedule.ShiftStartDate;
                    updateShift.ExpectedOnTime = shiftBO.ShiftSchedule.ShiftStartDate.Add(shiftBO.ShiftSchedule.ExpectedOnTime);
                    updateShift.ExpectedOffTime = shiftBO.ShiftSchedule.ShiftStartDate.Add(shiftBO.ShiftSchedule.ExpectedOffTime);
                    if (shiftBO.ShiftSchedule.ActualOnTime != null && shiftBO.ShiftSchedule.ActualOnDateTime != null)
                    {
                        updateShift.ActualOnTime = (DateTime)shiftBO.ShiftSchedule.ActualOnDateTime.Value.Add((TimeSpan)shiftBO.ShiftSchedule.ActualOnTime);
                    }
                    else
                    {
                        updateShift.ActualOnTime = null;
                    }
                    if (shiftBO.ShiftSchedule.ActualOffTime != null && shiftBO.ShiftSchedule.ActualOffDateTime != null)
                    {
                        updateShift.ActualOffTime = (DateTime)shiftBO.ShiftSchedule.ActualOffDateTime.Value.Add((TimeSpan)shiftBO.ShiftSchedule.ActualOffTime);
                    }
                    else
                    {
                        updateShift.ActualOffTime = null;
                    }
                    _db.SaveChanges();
                    msg = "Successfully Updated";
                    return true;
                }
                else
                {
                    msg = "Something Wrong";
                    return false;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

        }
        /// <summary>
        /// Get Weekly Shift List
        /// </summary>
        /// <param name="shiftSearch"></param>
        /// <returns></returns>
        public ShiftScheduleViewModel GetWeeklyShiftList(ShiftSearchViewModel shiftSearch)
        {

            ShiftScheduleViewModel shiftAddList = new ShiftScheduleViewModel();
            var shiftList = GetShiftScheduleList(shiftSearch);
            if (shiftSearch.SelectView == "Weekly" && shiftSearch.StartDate != null)
            {
                var daysEnd = shiftSearch.StartDate.Value.AddDays(6);
                var shiftWeekly = shiftList.Where(x => x.ShiftStartDate >= shiftSearch.StartDate && x.ShiftStartDate <= daysEnd).GroupBy(x => x.Employee).Select(g => new { Employee = g.Key, ShiftStartDate = g.Select(y => y.ShiftStartDate).ToList(), ShiftPattern = g.Select(z => z.ShiftPattern).ToList() });
                var conDate = shiftWeekly.Select(c => c.ShiftStartDate).ToList();
                var conEmp = shiftWeekly.Select(c => c.Employee).ToList();
                var conPattern = shiftWeekly.Select(c => c.ShiftPattern).ToList();
                shiftAddList.ResourceNameList = new List<WeeklyBO>();
                for (int i = 0; i < shiftWeekly.Select(c => c.Employee).ToList().Count; i++)
                {
                    WeeklyBO weekBO = new WeeklyBO();
                    weekBO.ResourceWeekly = conEmp[i];
                    weekBO.ShiftPatternList = conPattern[i];
                    weekBO.ShiftStartDateList = conDate[i];
                    shiftAddList.ResourceNameList.Add(weekBO);
                }
            }
            return shiftAddList;
        }
        /// <summary>
        /// Get Resource ShiftPattern
        /// </summary>
        /// <param name="shiftSchedule"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        public List<int> GetResourceShiftPattern(DateTime startDat, int empID)
        {
            var shiftPattern = _db.Resource_ShiftSchedule.Where(m => m.ShiftDate == startDat && m.EmployeeID == empID).Select(m => m.ShiftPatternID).ToList();
            return shiftPattern;
        }
        /// <summary>
        /// Get ShiftPattern ID
        /// </summary>
        /// <param name="shiftPatternID"></param>
        /// <returns></returns>
        public Resource_ShiftPattern GetShiftPatternID(int shiftPatternID)
        {
            var shiftPtt = _db.Resource_ShiftPattern.Where(x => x.ShiftPatternID == shiftPatternID).FirstOrDefault();
            return shiftPtt;
        }
        public bool SaveCSVFileData(ResourceShift resourceShift, out string msg)
        {
            msg = "";
            if (resourceShift != null)
            {
                Resource_ShiftSchedule resShift = new Resource_ShiftSchedule();
                resShift.EmployeeID = (int)resourceShift.EmployeeID;
                resShift.ShiftPatternID = (int)resourceShift.ShiftPatternID;
                resShift.ShiftDate = (DateTime)resourceShift.ShiftDate;
                resShift.ExpectedOffTime = (DateTime)resourceShift.ExpectedOffTime;
                resShift.ExpectedOnTime = (DateTime)resourceShift.ExpectedOnTime;
                resShift.ActualOnTime = resourceShift.ActualOnTime;
                resShift.ActualOffTime = resourceShift.ActualOffTime;
                resShift.BranchID = (int)resourceShift.BranchID;
                _db.Resource_ShiftSchedule.Add(resShift);
                _db.SaveChanges();
                msg = "Successfully Saved";
                return true;
            }
            return false;
        }
    }
}
