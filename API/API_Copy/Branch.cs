using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Branch
    {
        public class BranchRequest
        {
            public string orderTypeId { get; set; }
        }

        public class QRResponse
        {
            public string qrCode { get; set; }
        }

        public class BranchResponse
        {
            public List<BranchList> branchList { get; set; }
        }

        public class BranchListOpt2
        {
            public BranchListOpt2()
            {
            }

            public string branchId { get; set; }
            public string branchName { get; set; }
            public string address { get; set; }
            public string contactNo { get; set; }
            public string email { get; set; }
            public string location { get; set; }
            public string region { get; set; }
            public int isTax { get; set; }
            public string serviceTaxNo { get; set; }
            public string logitude { get; set; }
            public string latitude { get; set; }
            public List<DayWeek> dayWeek { get; set; }
        }

        public class BranchList
        {
            public BranchList()
            {
            }

            public string branchId { get; set; }
            public string branchName { get; set; }
            public string address { get; set; }
            public string contactNo { get; set; }
            public string email { get; set; }
            public string location { get; set; }
            public string region { get; set; }
            public int isTax { get; set; }
            public string serviceTaxNo { get; set; }
            public string logitude { get; set; }
            public string latitude { get; set; }
            public string operationHours { get; set; }
            public string qrCode { get; set; }
            public bool isClosed { get; set; }
            public double distanceFromOrigin { get; set; }
            public bool isNotApplicable { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsOutletOpenForOrder()
            {
                return isClosed == false && isNotApplicable == false;
            }
        }

        public class DayWeek
        {
            public DayWeek()
            {
            }

            public string dayOfWeek { get; set; }
            public string openPeriodType { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
        }

        public class BranchTSRequest
        {
            public string branchId { get; set; }
        }

        public class BranchTSResponse
        {
            public List<string> timeSlot { get; set; }
        }

        public class BranchOrderTypeExclusion
        {
            public string BranchId { get; set; }
            public string OrderTypeId { get; set; }
            public BranchOrderTypeExclusion() { }
        }

        public class HasUpdateResponse
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public string Version { get; set; }
        }

        public class MaintenanceResponse
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public bool IsOnMaintenance { get; set; }
            public bool ShowWarning { get; set; }
            public string WarningMessage { get; set; }
            public string MaintenanceMessage { get; set; }

            public MaintenanceResponse() { }
        }
    }
}