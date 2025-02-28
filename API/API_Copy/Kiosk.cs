using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Kiosk
    {
        //Base class
        //All kiosk request data must include these properties from KioskBaseRequest.
        public class KioskBaseRequest
        {
            public int ComponentId { get; set; }
            public string ComponentUniqueId { get; set; }
        }
        public class KioskCustomerBaseRequest : KioskBaseRequest
        {
            public int CustomerId { get; set; }
            public string AccessToken { get; set; }
        }
        //All kiosk response data must include properties from ResponseBase.


        public class KioskComponentSetting
        {
            public int AppSettingId { get; set; }
            public string AppSettingName { get; set; }
            public string AppSettingCategory { get; set; }
            public string DataType { get; set; }
            public bool IsEnableApplyLocation { get; set; }
            public string Remarks { get; set; }
            public string AppSettingValue { get; set; }
        }
        public class KioskProfileRequest : KioskBaseRequest
        {

        }
        public class KioskProfileResponse : ResponseBase
        {
            public int ComponentId { get; set; }
            public string ComponentUniqueId { get; set; }
            public string ComponentCode { get; set; }
            public string ComponentName { get; set; }
            public string ComponentConfig { get; set; }
            public string BranchId { get; set; }
            public string BranchName { get; set; }
            public List<KioskComponentSetting> AppSettings { get; set; }
        }

        public class VerifyProfileRequest : KioskBaseRequest
        {
            public int CustomerId { get; set; }
            public string AccessToken { get; set; }
        }
        public class VerifyProfileResponse : ResponseBase
        {
            public int CustomerId { get; set; }
            public string AccessToken { get; set; }
            public Loyalty.FullProfile CustomerProfile { get; set; }
        }

        public class GetCustomerPromotionsRequest : KioskCustomerBaseRequest
        {

        }
        public class GetCustomerPromotionsResponse : ResponseBase
        {
            public List<Loyalty.RedemptionItem> VoucherList { get; set; }
        }

        public class ReadAndUsePromotionRequest : KioskCustomerBaseRequest
        {
            public bool IsPromoCode { get; set; }
            public string VoucherId { get; set; } //To be passed here is the EncodedUniqueId
            public string PromoCodeValue { get; set; }
            public int OrderTypeId { get; set; }
            public string BranchId { get; set; }
            public List<Order.OrderReward> AddedVoucherList { get; set; }
            public List<Order.OrderMenuList> CustomerOrderCart { get; set; }
        }
        public class ReadAndUsePromotionResponse : ResponseBase
        {
            public Order.OrderReward VoucherItem { get; set; }
            public Order.OrderMenuList MenuItemPromotion { get; set; }
        }

        public class AutoReviewOrderRequest : KioskBaseRequest
        {
            public Order.OrderRequest CustomerOrder { get; set; }
        }
        public class AutoReviewOrderResponse : ResponseBase
        {
            public Order.OrderMenuList MenuToAdd { get; set; }
            public Order.OrderMenuList MenuToRemove { get; set; }
            public Order.OrderReward PromotionToAdd { get; set; }
            public Order.OrderReward PromotionToRemove { get; set; }
        }

        public class KioskPaymentType
        {
            public int PaymentId { get; set; }
            public string PaymentGroup { get; set; }
            public string PaymentCode { get; set; }
            public string PaymentName { get; set; }
            public int Sequence { get; set; }
            public string VendorPaymentId { get; set; }
        }
        public class PaymentKioskTypeRequest : KioskBaseRequest
        {

        }
        public class PaymentKioskTypeResponse : ResponseBase
        {
            public List<KioskPaymentType> PaymentList { get; set; }
        }

        public class InitialKioskOrderRequest : KioskBaseRequest
        {
            public Order.OrderRequest CustomerOrder { get; set; }
        }
        public class InitialKioskOrderResponse : ResponseBase
        {
            public string OrderId { get; set; }
            public string DisplayOrderNumber { get; set; }
            public List<Order.OrderItemToRemove> InvalidMenus { get; set; }
        }

        public class PostKioskOrderRequest : KioskBaseRequest
        {
            public Order.OrderRequest CustomerOrder { get; set; }
            public string PaymentTransactionId { get; set; }
        }
        public class PostKioskOrderResponse : ResponseBase
        {
            public string OrderId { get; set; }
            public string DisplayOrderNumber { get; set; }
            public int EarnPoints { get; set; }
            public int TotalPoints { get; set; }
        }

        public class MenuMbaItem
        {
            public int Id { get; set; }
            public string Antecedents { get; set; }
            public string Consequents { get; set; }
            public double AntecedentSupport { get; set; }
            public double ConsequentSupport { get; set; }
            public double Support { get; set; }
            public double Confidence { get; set; }
            public double Lift { get; set; }
            public double Leverage { get; set; }
            public double Conviction { get; set; }
            public double ZhangsMetric { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
        public class GetMenuMbaRequest : KioskBaseRequest
        {

        }
        public class GetMenuMbaResponse : ResponseBase
        {
            public List<MenuMbaItem> ItemList { get; set; }
        }
    }
}