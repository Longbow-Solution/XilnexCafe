using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Delivery
    {
        public class DeliveryHistoryResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public List<DeliveryOrderItem> Deliveries { get; set; }
            public DeliveryHistoryResponse() { }
        }

        public class LatestDeliveryResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public DeliveryOrderItem LatestDelivery { get; set; }
        }

        public class SubmitFeedbackRequest
        {
            public int CustomerId { get; set; }
            public string Token { get; set; }
            public int TxId { get; set; }
            public int DeliveryDetailTxId { get; set; }
            public int FoodRating { get; set; }
            public int DeliveryRating { get; set; }
            public string FeedbackContent { get; set; }
            public SubmitFeedbackRequest() { }
        }
        public class SubmitFeedbackResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public SubmitFeedbackResponse() { }
        }

        public class CancelDeliveryCustomerRequest
        {
            public int CustomerId { get; set; }
            public string Token { get; set; }
            public int TxId { get; set; }
            public int DeliveryDetailTxId { get; set; }
            public string Reason { get; set; }
            public int ReasonCode { get; set; }
            public CancelDeliveryCustomerRequest() { }
        }
        public class CancelDeliveryBranchRequest
        {
            public string BranchId { get; set; }
            public int UserId { get; set; }
            public int CustomerId { get; set; }
            public int TxId { get; set; }
            public int DeliveryDetailTxId { get; set; }
            public string Reason { get; set; }
            CancelDeliveryBranchRequest() { }
        }
        public class CancelDeliveryResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public CancelDeliveryResponse() { }
        }

        public class DeliveryOrderItem
        {
            public int CustId { get; set; }
            public int TxId { get; set; }
            public int DeliTxId { get; set; }
            public string OrderNo { get; set; }
            public string ShortOrderNo { get; set; }
            public string ExternalOrderId { get; set; }
            public string TxDate { get; set; }
            public string DeliveryTime { get; set; }
            public int WaitingTimeCountdown { get; set; }
            public string WaitingTimeDescription { get; set; }
            public int AddressId { get; set; }
            public string AddressBody { get; set; }
            public string BranchId { get; set; }
            public string BranchName { get; set; }
            public string BranchContactNo { get; set; }
            public int RiderId { get; set; }
            public string RiderName { get; set; }
            public string RiderPhoneNumber { get; set; }
            public string RiderPlateNumber { get; set; }
            public string StatusCode { get; set; }
            public string StatusName { get; set; }
            public string TrackingUrl { get; set; }
            public bool ShowRider { get; set; }
            public bool ShowFeedback { get; set; }
            public bool ShowCancel { get; set; }
            public bool ShowRefund { get; set; }
            public bool ShowOrderReceived { get; set; }
            public bool ShowAltOrderStatus { get; set; }
            public bool ShowTrackingLink { get; set; }
            public bool ShowCountdown { get; set; }
            public bool ShowNotificationBar { get; set; }
            public DeliveryOrderItem() { }

            public void CalculateWaitingTime(DateTime txnDt, DateTime pickupDt, DateTime deliveryDt, string statusCode)
            {
                DateTime targetDt;
                DateTime currentDt = DateTime.Now;
                //Check for null string value of statusCode before proceed.
                if (string.IsNullOrEmpty(statusCode))
                {
                    statusCode = "D";
                }
                switch (statusCode)
                {
                    case "P":
                    case "R":
                    case "RP":
                        WaitingTimeDescription = "Order will be picked up by rider in";
                        targetDt = pickupDt;
                        break;
                    case "OT":
                        WaitingTimeDescription = "Rider is heading to you in";
                        targetDt = deliveryDt;
                        break;
                    case "S":
                    case "A":
                    case "AR":
                        WaitingTimeDescription = "ETA";
                        targetDt = deliveryDt;
                        break;
                    case "D":
                    case "C":
                    default:
                        WaitingTimeDescription = "ETA";
                        targetDt = txnDt;
                        break;
                }
                TimeSpan remainingTimespan = targetDt - currentDt;
                WaitingTimeCountdown = (int)remainingTimespan.TotalSeconds;
                if (WaitingTimeCountdown < 0)
                {
                    WaitingTimeCountdown = 0;
                }
            }
        }
        public class DeliverySaleDetail
        {
            public int TxDeliveryId { get; set; }
            public int TxSalesId { get; set; }
            public int CustId { get; set; }
            public string ExternalOrderId { get; set; }
            public int? CustAddressId { get; set; }
            public int? RiderId { get; set; }
            public DateTime? PickupDate { get; set; }
            public DateTime? SendDate { get; set; }
            public string Status { get; set; }
            public string ExternalStatus { get; set; }
            public string NoteToDriver { get; set; }
            public int? FoodRating { get; set; }
            public int? DeliveryRating { get; set; }
            public string CustomerFeedback { get; set; }
            public DeliverySaleDetail() { }
        }

        public class Rider
        {
            public int Id { get; set; }
            public string PhoneNumber { get; set; }
            public string Type { get; set; }
            public string Nickname { get; set; }
            public string FullName { get; set; }
            public string PlateNumber { get; set; }
            public string ExternalRiderId { get; set; }
            public string UserToken { get; set; }
            public string DeviceToken { get; set; }
            public bool IsOnline { get; set; }
            public string Status { get; set; }
            public Rider() { }
        }

        public class GetNearestBranchResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public double DeliveryDisplacement { get; set; }
            public Branch.BranchList NearestBranch { get; set; }
        }

        public class DeliveryIntegrationStatusCheck
        {
            public string Code { get; set; }
            public string Remarks { get; set; }
            public string HelperPropJson { get; set; }
            public string LastErrorMessage { get; set; }
            public bool PandaGoTokenStatus { get; set; }
            public string PandaGoTokenErrMsg { get; set; }
            public bool PandaGoFeeTimeEstimationStatus { get; set; }
            public string PandaGoFeeTimeEstimationErrMsg { get; set; }
            public string PandaGoFeeEstimationOutput { get; set; }
            public string PandaGoPickupTimeEstimationOutput { get; set; }
            public string PandaGoDeliveryTimeEstimationOutput { get; set; }
        }
    }
}