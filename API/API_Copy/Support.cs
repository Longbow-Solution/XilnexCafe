using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Support
    {
        public class SupportCodeResponse
        {
            public List<SupportCodeList> supportCatList { get; set; }
            public List<SupportCodeList> rewardTypeList { get; set; }
        }

        public class SupportCodeList
        {
            public SupportCodeList()
            {
            }

            public string listId { get; set; }
            public string listName { get; set; }
        }

        public class SupportRequest
        {
            public int clientId { get; set; }
            public string BranchId { get; set; }
            public string OrderNo { get; set; }
            public DateTime TxDate { get; set; }
            public decimal TxAmt { get; set; }
            public int IssueCatId { get; set; }
            public int RewardTypeId { get; set; }
            public string Remarks { get; set; }
            public string FullName { get; set; }
            public string PhoneNo { get; set; }
            public string Email { get; set; }
        }

        public class SupportResponse
        {
            public int ReportNo { get; set; }
            public string Message { get; set; }
        }

        public class CustomerDataReviewResponse : ResponseBase
        {
            public Loyalty.FullProfile Profile { get; set; }
            public List<Order.OrderDetailHistory> OrderHistory { get; set; }
            public List<Loyalty.RedemptionItem> VoucherList { get; set; }
        }

        public class BranchDataReviewResponse : ResponseBase
        {
            public Branch.BranchList Branch { get; set; }
            public bool IsDataCompleteForDelivery { get; set; }
            public bool IsCoordinateSetCorrectly { get; set; }
        }

        public class UserPasswordResetRequest
        {
            public string PhoneNumber { get; set; }
        }

        public class UserPasswordResetResponse : ResponseBase
        {
            public string PhoneNumber { get; set; }
            public string TemporaryPassword { get; set; }
        }

        public class PromptMessageEntry
        {
            public int EntryId { get; set; }
            public string EntryName { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string PositiveText { get; set; }
            public string NegativeText { get; set; }

            public PromptMessageEntry() { }
            public PromptMessageEntry(int id, string name, string title, string message, string yesText = "OK", string noText = "")
            {
                EntryId = id;
                EntryName = name;
                Title = title;
                Message = message;
                PositiveText = yesText;
                NegativeText = noText;
            }
        }

        public class RetrievePromptMessagesResponse : ResponseBase
        {
            public List<PromptMessageEntry> Entries { get; set; }
        }

        public class HeartbeatRequest
        {
            public int CustomerId { get; set; }
            public string CustomerPhone { get; set; }
            /// <summary>
            /// Another term of User Token.
            /// </summary>
            public string AccessCode { get; set; }
            public string Platform { get; set; }
        }
        public class HeartbeatResponse : ResponseBase
        {

        }
    }
}