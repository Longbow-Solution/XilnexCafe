using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Loyalty
    {
        #region Enumerators

        public enum CustomerType
        {
            Pre,
            Temp,
            Basic
        }

        public enum RedemptionType
        {
            Unknown,
            Redeem, //Just for redeeming reward only.
            Voucher, //Item or RedemptionItemType.Item voucher goes here.
            Promo,
            EmployeeVoucher, //Specifially, this is an employee meal voucher.
            GiftVoucher,
            PointVoucher,
            FirstDeliveryVoucher, //Delivery discount voucher by percentage, but this is a one-time use.
            DeliveryPercentVoucher, //Delivery discount voucher by percentage
            DeliveryAmountVoucher, //Delivery discount voucher by amount
            CartVoucherAmount,
            CartVoucherPercentage,
            DeliveryAmountPromo, //Delivery discount promo code by amount
            DeliveryPercentPromo, //Delivery discount promo code by percentage
            CartPromoAmount,
            CartPromoPercent,
            ItemVoucher,
        }

        public enum RedemptionItemType
        {
            Unknown,
            Item,
            Amount,
            Percentage,
            DeliveryAmount,
            DeliveryPercentage,
            TenderAmount,
            TenderPercentage,
            Point
        }

        public enum RedemptionAction
        {
            Redeem,
            Use,
            Cancel,
            Sale
        }

        #endregion

        #region Authentication, Profile, Token, OTP

        public class ProfileRequest
        {
            public ProfileRequest() { }

            public string Token { get; set; }
            public string CustomerId { get; set; }
            public string PhoneNumber { get; set; }
            public string Salutation { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DOB { get; set; }
            public string ReferralCode { get; set; }
            public string EmailAddress { get; set; }
            public string PlainTextPassword { get; set; }
        }



        public class FullProfile
        {
            public FullProfile() { }
            public string CustomerId { get; set; }
            public string CustomerCode { get; set; }
            public string PhoneNumber { get; set; }
            public string Salutation { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DOB { get; set; }
            public string EmailAddress { get; set; }
            public string MemberLevelId { get; set; }
            public string MemberLevel { get; set; }
            public DateTime LevelExpired { get; set; }
            public int TotalPoint { get; set; }
            public int AccPoint { get; set; }
            public string MyReferralCode { get; set; }
            public string AccessToken { get; set; }

            public List<AddressObj> Address { get; set; }
            public LoyalProfile Loyal { get; set; }
        }

        public class LoyalProfile
        {
            public LoyalProfile() { }
            public string MemberTitle { get; set; }
            public string MemberDesc1 { get; set; }
            public string MemberDesc2 { get; set; }
            public int MemberIndex { get; set; }
            public string Point { get; set; }
            public string PointDesc { get; set; }
            public string PointTitle { get; set; }
            public string ExpiredMessage { get; set; }
        }

        public class ProfileResponse
        {
            public ProfileResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string Token { get; set; }
            public FullProfile Profile { get; set; }
        }

        public class UpdateProfileRequest
        {
            public UpdateProfileRequest() { }

            public string Token { get; set; }
            public string CustomerId { get; set; }
            public string PhoneNumber { get; set; }
            public string Salutation { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DOB { get; set; }
            public string EmailAddress { get; set; }
        }

        public class UpdateProfileResponse
        {
            public UpdateProfileResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public FullProfile Profile { get; set; }
        }



        public class ForgotPasswordRequest
        {
            public ForgotPasswordRequest() { }

            public string PhoneNumber { get; set; }

        }

        public class ForgotPasswordResponse
        {
            public ForgotPasswordResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string TempId { get; set; }
            public string OTP { get; set; }

            public string Token { get; set; }

        }

        public class ForgotPasswordChangeRequest
        {
            public ForgotPasswordChangeRequest() { }
            public string CustomerId { get; set; }
            public string TempId { get; set; }
            public string PlainTextPassword { get; set; }
            public string PhoneNumber { get; set; }

        }

        public class ForgotPasswordChangeResponse
        {
            public ForgotPasswordChangeResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }

            public string Token { get; set; }

        }

        public class ChangePasswordRequest
        {
            public ChangePasswordRequest() { }
            public string Token { get; set; }
            public string CustomerId { get; set; }
            public string OriPlanTextPassowrd { get; set; }
            public string NewPlainTextPassword { get; set; }

        }

        public class ChangePasswordResponse
        {
            public ChangePasswordResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }

            public string Token { get; set; }

        }

        public class AddDeviceTokenRequest
        {
            public AddDeviceTokenRequest() { }
            public string Token { get; set; }
            public string CustomerId { get; set; }
            public string DeviceToken { get; set; }
        }

        public class ClearDeviceTokenRequest
        {
            public ClearDeviceTokenRequest() { }
            public string Token { get; set; }
            public string CustomerId { get; set; }
        }

        public class AddDeviceTokenResponse
        {
            public AddDeviceTokenResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }

            public string Token { get; set; }

        }

        public class ClearDeviceTokenResponse
        {
            public ClearDeviceTokenResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }

            public string Token { get; set; }

        }

        public class HeaderTokenRequest
        {
            public HeaderTokenRequest() { }

            public string Role { get; set; }

        }

        public class HeaderTokenResponse
        {
            public HeaderTokenResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string xKeyToken { get; set; }

        }

        public class AuthenticationRequest
        {
            public AuthenticationRequest() { }

            //AuthenticationType:Login, Register
            //AuthenticationMtd:Mobile, Facebook, Google, Apple, Token, TNG
            //CredentialId:PhoneNumber, Email of Third Party Authentication, Token

            public string AuthenticationType { get; set; }
            public string AuthenticationMtd { get; set; }
            public string CredentialId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string CredentialConfig { get; set; }
            public string DeviceToken { get; set; }
            public bool IsHuawei { get; set; }
        }

        public class AuthenticationResponse
        {
            public AuthenticationResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerId { get; set; }
            public string TempId { get; set; }
            public string OTP { get; set; }

            public string Token { get; set; }

            public FullProfile Profile { get; set; }

        }

        public class GetProfileRequest
        {
            public GetProfileRequest() { }


            public string Token { get; set; }
            public string CustomerId { get; set; }

        }

        public class GetProfileResponse
        {
            public GetProfileResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public FullProfile Profile { get; set; }

        }

        public class VerifyPhoneNumberRequest
        {
            public VerifyPhoneNumberRequest() { }

            //AuthenticationType:Login, Register
            //AuthenticationMtd:Mobile, Facebook, Google, Apple, Token
            //CredentialId:PhoneNumber, Email of Third Party Authentication, Token

            public string TempId { get; set; }
            public string PhoneNumber { get; set; }

        }

        public class VerifyPhoneNumberResponse
        {
            public VerifyPhoneNumberResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerId { get; set; }
            public string TempId { get; set; }
            public string OTP { get; set; }
            public string Token { get; set; }
            public bool ProceedPassword { get; set; }

        }

        public class VerifyOTPRequest
        {
            public VerifyOTPRequest() { }
            public string PhoneNumber { get; set; }
            public string CustomerId { get; set; }
            public string tempId { get; set; }
            public string OTPNumber { get; set; }
        }

        public class VerifyOTPResponse
        {
            public VerifyOTPResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string Token { get; set; }
            public bool ProceedPassword { get; set; }
        }

        public class RefreshOTPRequest
        {
            public RefreshOTPRequest() { }

            public string PhoneNumber { get; set; }
            public string CustomerId { get; set; }
            public string tempId { get; set; }
        }

        public class RefreshOTPResponse
        {
            public RefreshOTPResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string TempId { get; set; }
            public string OTPNumber { get; set; }
        }

        public class CustomerProfile
        {
            public CustomerProfile() { }
            public string PhoneNumber { get; set; }
            public string Salutation { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public DateTime DOB { get; set; }
            public string EmailAddress { get; set; }
            public string TierLevel { get; set; }
        }

        public class ProfileGetAccessRequest
        {
            public int CustomerId { get; set; }
            public string Token { get; set; }
        }

        public class ProfileGetAccessResponse : ResponseBase
        {
            public string AccessToken { get; set; }
        }

        #endregion

        #region Notification

        public class NotificationRequest
        {
            public enum PresentType
            {
                General,
                Ordering
            }

            public NotificationRequest() { }

            public string MessageId { get; set; }
            public string CustomerId { get; set; }
            public string TierLevelTarget { get; set; }
            public PresentType NotificationType { get; set; }
            public string NotificationTitle { get; set; }
            public string NotificationMessage { get; set; }
            public string MessageTitle { get; set; }
            public string MessageSummary { get; set; }
            public string MessageTimeDate { get; set; }
            public string MessageContent { get; set; }
            public string ImageUrl { get; set; }

            public override string ToString()
            {
                return string.Format("CustomerId: {0}\nTierLevelTarget: {1}\nNotificationType: {2}\nNotificationTitle: {3}\nNotificationMessage: {4}\nMessageTimeDate: {5}\nMessageTitle: {6}\nMessageSummary: {7}\nMessageContent:{8}\nImageUrl:{9}",
                    CustomerId, TierLevelTarget, NotificationType.ToString(), NotificationTitle, NotificationMessage, MessageTimeDate, MessageTitle, MessageSummary, MessageContent, ImageUrl);
            }
        }

        public class NotificationTimedRequest : NotificationRequest
        {
            public DateTime NotificationDateTime { get; set; }
            public bool MustSaveInDb { get; set; }

            public NotificationTimedRequest() : base()
            {
                if (NotificationDateTime == DateTime.MinValue)
                {
                    NotificationDateTime = DateTime.Now;
                }
            }

            public override string ToString()
            {
                return string.Format("CustomerId: {0}\nTierLevelTarget: {1}\nNotificationType: {2}\nNotificationTitle: {3}\nNotificationMessage: {4}\nMessageTimeDate: {5}\nMessageTitle: {6}\nMessageSummary: {7}\nMessageContent:{8}\nNotificationDateTime: {9}\nMustSaveInDb: {10}",
                    CustomerId, TierLevelTarget, NotificationType.ToString(), NotificationTitle, NotificationMessage, MessageTimeDate, MessageTitle, MessageSummary, MessageContent, NotificationDateTime.ToString("yyyy-MM-dd HH:mm:ss"), MustSaveInDb.ToString());
            }
        }

        public class Notification
        {
            public Notification() { }

            public string MessageId { get; set; }
            public DateTime EffectiveDate { get; set; }
            public string NotificationTitle { get; set; }
            public string NotificationMessage { get; set; }
            public string MessageTitle { get; set; }
            public string MessageSummary { get; set; }
            public string MessageTimeDate { get; set; }
            public string MessageContent { get; set; }
            public string IsRead { get; set; }
            public bool IsHighPriority { get; set; }
            public string ImageUrl { get; set; }
            public string UrlLink { get; set; }

            public NotificationRequest GetAsRequestObj(int customerId = 0, string tierTarget = "0")
            {
                return new NotificationRequest()
                {
                    NotificationTitle = NotificationTitle,
                    NotificationMessage = NotificationMessage,
                    MessageTitle = MessageTitle,
                    MessageSummary = MessageSummary,
                    MessageContent = MessageContent,
                    MessageTimeDate = MessageTimeDate,
                    NotificationType = IsHighPriority == true ? NotificationRequest.PresentType.Ordering : NotificationRequest.PresentType.General,
                    TierLevelTarget = tierTarget,
                    CustomerId = customerId.ToString(),
                    MessageId = MessageId
                };
            }
        }

        public class ReadNotificationRequest
        {
            public ReadNotificationRequest() { }

            public string CustomerId { get; set; }
            public string MessageId { get; set; }
        }

        public class ReadNotificationResponse
        {
            public ReadNotificationResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
        }

        public class ReadAllNotificationRequest
        {
            public ReadAllNotificationRequest() { }
            public string CustomerId { get; set; }
        }

        public class ReadAllNotificationResponse
        {
            public ReadAllNotificationResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
        }

        public class NotificationResponse
        {
            public NotificationResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
        }

        public class NotificationListRequest
        {
            public NotificationListRequest() { }

            public string CustomerId { get; set; }
            public string Token { get; set; }
            public int? IsNewApp { get; set; }
        }

        public class NotificationListResponse
        {
            public NotificationListResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public List<Notification> NotificationList { get; set; }

        }

        #endregion

        #region Redemption

        public class RedeemActionResquest
        {
            public RedeemActionResquest() { }

            public string EntityCId { get; set; }
            public string BranchId { get; set; }
            public string Ogi { get; set; }
            public string RedeemAction { get; set; }
            public int TxSalesId { get; set; }
            public RedemptionItem Item { get; set; }
            public List<EmpVoucherDetail> EmpVoucherDetailList { get; set; }
        }

        public class GetVoucherRequest
        {
            public GetVoucherRequest() { }

            public string CustomerId { get; set; }
            public string Token { get; set; }
        }

        public class GetVoucherResponse
        {
            public GetVoucherResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public List<RedemptionItem> Items { get; set; }
        }

        public class RedeemActionResponse
        {
            public RedeemActionResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public RedemptionItem Item { get; set; }

        }

        public class PromoCodeRequest
        {
            public PromoCodeRequest() { }

            public string CustomerId { get; set; }
            public string Token { get; set; }
            public string PromoCode { get; set; }
        }

        public class PromoCodeResponse
        {
            public PromoCodeResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string Token { get; set; }
        }

        public class RedemptionListRequest
        {
            public RedemptionListRequest() { }
            public string Token { get; set; }
            public string CustomerId { get; set; }
        }

        public class Redemption
        {
            public Redemption() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string RedemptionName { get; set; }
            public List<RedemptionItem> Items { get; set; }
        }

        public class RedemptionItem
        {
            public RedemptionItem() { if (MaxAmount == decimal.Zero) { MaxAmount = 9999m; } }
            public string Code { get; set; }
            public string Type { get; set; }
            public string ItemType { get; set; }
            public string RedeemTxId { get; set; }
            public string RedeemId { get; set; }
            public string SerialCode { get; set; } //For Employ
            public decimal Amount { get; set; }
            public decimal MaxAmount { get; set; }
            public string CatId { get; set; }
            public string ItemId { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string ItemImg { get; set; }
            public string Points { get; set; }
            public string IsAvailable { get; set; }
            public DateTime ExpiredDate { get; set; }
            public int Qty { get; set; }
            public string TermsAndConditions { get; set; }
            public bool HasExpired { get; set; }
            public bool IsVoucher { get; set; }
            public bool IsPromoCode { get; set; }
            public bool IsDiscount { get; set; }
            public bool IsItem { get; set; }
            public bool ShowTerms { get; set; }
            public bool ShowQty { get; set; }

            public string EncodedUniqueId { get; set; }

            public RedemptionType GetRedemptionType()
            {
                if (!string.IsNullOrEmpty(Type))
                {
                    if (Enum.TryParse(Type, out RedemptionType parsedVal))
                    {
                        return parsedVal;
                    }
                    else
                    {
                        return RedemptionType.Unknown;
                    }
                }
                else
                {
                    return RedemptionType.Unknown;
                }
            }
            /// <summary>
            /// Get the Redemption Item Type of the reward.
            /// </summary>
            /// <returns></returns>
            public RedemptionItemType GetRedemptionItemType()
            {
                if (!string.IsNullOrEmpty(ItemType))
                {
                    if (Enum.TryParse(ItemType, out RedemptionItemType parsedVal))
                    {
                        return parsedVal;
                    }
                    else
                    {
                        return RedemptionItemType.Unknown;
                    }
                }
                else
                {
                    return RedemptionItemType.Unknown;
                }
            }

            private void DetermineIfExpired()
            {
                if (ExpiredDate != default)
                {
                    HasExpired = DateTime.Now > ExpiredDate;
                }
                else
                {
                    HasExpired = false;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public void CompleteSetup()
            {
                DetermineIfExpired();
                switch (GetRedemptionType())
                {
                    case RedemptionType.Voucher:
                    case RedemptionType.EmployeeVoucher:
                    case RedemptionType.GiftVoucher:
                    case RedemptionType.PointVoucher:
                    case RedemptionType.FirstDeliveryVoucher:
                    case RedemptionType.DeliveryPercentVoucher:
                    case RedemptionType.DeliveryAmountVoucher:
                    case RedemptionType.CartVoucherAmount:
                    case RedemptionType.CartVoucherPercentage:
                    case RedemptionType.ItemVoucher:
                        IsVoucher = true;
                        break;
                    case RedemptionType.Unknown:
                    case RedemptionType.Redeem:
                    case RedemptionType.Promo:
                    default:
                        IsVoucher = false;
                        break;
                }
                IsPromoCode = GetRedemptionType() == RedemptionType.Promo;
                IsDiscount = GetRedemptionItemType() == RedemptionItemType.Amount || GetRedemptionItemType() == RedemptionItemType.Percentage;
                IsItem = GetRedemptionItemType() == RedemptionItemType.Item;
            }

            /// <summary>
            /// Generate encoded ID of the voucher.
            /// </summary>
            /// <param name="customerId">Customer ID of the owner of this promotion item.</param>
            public void GenerateEncodedUniqueId(string customerId)
            {
                string rawData = $"customerId={customerId};type={Type};itemType={ItemType};parentId={RedeemId};txId={RedeemTxId}";
                using (var md5Inst = System.Security.Cryptography.MD5.Create())
                {
                    var rawEncoded = System.Text.Encoding.UTF8.GetBytes(rawData);
                    var encodedVal = md5Inst.ComputeHash(rawEncoded);
                    EncodedUniqueId = Convert.ToBase64String(encodedVal);
                }
            }
        }

        public class RedemptionHistRequest
        {
            public RedemptionHistRequest() { }

            public string CustomerId { get; set; }
            public string Token { get; set; }
        }

        public class RedemptionHistResponse
        {
            public RedemptionHistResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public List<RedemptionHistItem> HistItem { get; set; }
        }

        public class RedemptionHistItem
        {
            public RedemptionHistItem() { }

            public string RedemptionName { get; set; }
            public string RedemptionPoints { get; set; }
            public string RedemptionStatus { get; set; }
            public string RedemptionDate { get; set; }
        }

        public class RedemptionHistRawItem
        {
            public RedemptionHistRawItem() { }

            public string RedemptionName { get; set; }
            public string RedemptionPoints { get; set; }
            public string RedemptionStatus { get; set; }
            public DateTime RedemptionDate { get; set; }
        }

        public class VoucherCheckRequest
        {
            public string CustomerId { get; set; }
            public int OrderTypeId { get; set; }
            public string BranchId { get; set; }
            public DateTime CollectionTime { get; set; }
            public Order.OrderReward VoucherToBeAdded { get; set; }
            public List<Order.OrderReward> AddedVoucherList { get; set; }
            public List<Order.OrderMenuList> CustomerOrderCart { get; set; }
        }

        public class VoucherCheckResponse
        {
            public string Code { get; set; }
            public string Remark { get; set; }
            public string Title { get; set; }
        }

        public class VoucherPromoResponse
        {
            public string Code { get; set; }
            public string Remark { get; set; }
            public string Title { get; set; }
            public List<Order.OrderReward> VouchersToBeAdded { get; set; }
            public List<Order.OrderReward> VouchersToBeRemoved { get; set; }
        }

        #endregion

        #region Employee Voucher

        public class EmpVoucherDetail
        {
            public EmpVoucherDetail() { }

            public int VoucherTxId { get; set; }
            public string UniqCode { get; set; }
            public DateTime EndDate { get; set; }
            public string VoucherAmt { get; set; }
            //public string RedemptionDate { get; set; }
        }

        public class EmpVoucherTransferRequest
        {
            public EmpVoucherTransferRequest() { }
            public string CustomerId { get; set; }
            public string ReceiveStaffId { get; set; }
            public string ReceivePhoneNumber { get; set; }
            public string Code { get; set; }
            public string Token { get; set; }
            public string Action { get; set; }
            public List<EmpVoucherDetail> EmpVoucherDetailList { get; set; }
        }

        public class EmpVoucherTransferResponse
        {
            public EmpVoucherTransferResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string ReceiveStaffId { get; set; }
            public string ReceivePhoneNumber { get; set; }
            public string UniqCode { get; set; }
            public decimal Amount { get; set; }

        }

        public class EmpVoucherDetailRequest
        {
            public EmpVoucherDetailRequest() { }
            public string VoucherCd { get; set; }
            public string CustomerId { get; set; }
        }

        public class EmpVoucherDetailResponse
        {
            public EmpVoucherDetailResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public List<EmpVoucherDetail> EmpVoucherDetailList { get; set; }

        }

        public class EmployeeContactResponse
        {
            public List<EmployeeContactList> contactList { get; set; }
        }

        public class EmployeeContactList
        {
            public EmployeeContactList()
            {
            }

            public string PhoneNumber { get; set; }
            public string FullName { get; set; }
            public string CustomerId { get; set; }
            public string Id { get; set; }
        }

        #endregion

        #region Gift Voucher

        public class GiftVoucherDetail
        {
            public int VoucherTxId { get; set; }
            public string UniqueCode { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string Amount { get; set; }
        }

        public class GiftVoucherDetailRequest
        {
            public int VoucherId { get; set; }
            public string VoucherCode { get; set; }
            public string CustomerId { get; set; }
        }

        public class GiftVoucherDetailResponse
        {
            public string Code { get; set; }
            public string Remark { get; set; }
            public string Title { get; set; }
            public List<GiftVoucherDetail> VoucherList { get; set; }
        }

        public class GiftVoucherTransferRequest
        {
            public string CustomerId { get; set; }
            public string TargetPhoneNumber { get; set; }
            public string VoucherId { get; set; }
            public string VoucherCode { get; set; } //The Code of tblUserReward
            public string Token { get; set; }
            public string Action { get; set; } //Must always be 'Transfer'
            public List<GiftVoucherDetail> VoucherList { get; set; }
        }

        public class GiftVoucherTransferResponse
        {
            public string Code { get; set; }
            public string Remark { get; set; }
            public string Title { get; set; }
        }

        #endregion

        #region Account Check and Deletion

        public class AccountDeletionRequest
        {
            public AccountDeletionRequest() { }
            public string CustomerId { get; set; }
            public string PhoneNumber { get; set; }
            public string Token { get; set; }
        }
        public class AccountDeletionResponse
        {
            public AccountDeletionResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
        }

        public class AccountCheckRequest
        {
            public AccountCheckRequest() { }
            public string PhoneNumber { get; set; }
        }
        public class AccountCheckResponse
        {
            public AccountCheckResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string PhoneNumber { get; set; }
            public string CustomerId { get; set; }
            public string CustomerCode { get; set; }
            public string Status { get; set; }
        }

        #endregion

        #region Address

        public class UpdateAddressRequest
        {
            public UpdateAddressRequest() { }

            public string Token { get; set; }
            public string IsDefault { get; set; }
            public string CustomerId { get; set; }
            public string AddressId { get; set; }
            public string Address { get; set; }
            public string AddressName { get; set; }
            public string Logtitude { get; set; }
            public string Latitude { get; set; }
            public string AddressDetail { get; set; }
            public string NoteToDriver { get; set; }
            public string ContactName { get; set; }
            public string ContactNumber { get; set; }
            public string Remark { get; set; }
        }

        public class UpdateAddressResponse
        {
            public UpdateAddressResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string AddressId { get; set; }
            public FullProfile Profile { get; set; }
        }

        public class AddAddressRequest
        {
            public AddAddressRequest() { }

            public string Token { get; set; }
            public string IsDefault { get; set; }
            public string CustomerId { get; set; }
            public string AddressName { get; set; }
            public string Address { get; set; }
            public string Logtitude { get; set; }
            public string Latitude { get; set; }
            public string AddressDetail { get; set; }
            public string NoteToDriver { get; set; }
            public string ContactName { get; set; }
            public string ContactNumber { get; set; }
            public string Remark { get; set; }
        }

        public class AddAddressResponse
        {
            public AddAddressResponse() { }

            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public string AddressId { get; set; }
            public FullProfile Profile { get; set; }
        }

        public class AddressObj
        {
            public AddressObj() { }

            public string IsDefault { get; set; }
            public string AddressId { get; set; }
            public string CustomerId { get; set; }
            public string AddressName { get; set; }
            public string Address { get; set; }
            public string Longtitude { get; set; }
            public string Latitude { get; set; }
            public string AddressDetail { get; set; }
            public string NoteToDriver { get; set; }
            public string ContactName { get; set; }
            public string ContactNumber { get; set; }
            public string Remark { get; set; }
        }

        public class RemoveAddressRequest
        {
            public RemoveAddressRequest() { }
            public string CustomerId { get; set; }
            public string AddressId { get; set; }
            public string Token { get; set; }
        }
        public class RemoveAddressResponse
        {
            public RemoveAddressResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public FullProfile Profile { get; set; }
        }

        public class RemoveManyAddressRequest
        {
            public RemoveManyAddressRequest() { }
            public string CustomerId { get; set; }
            public List<string> AddressId { get; set; }
            public string Token { get; set; }
        }
        public class RemoveManyAddressResponse
        {
            public RemoveManyAddressResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public FullProfile Profile { get; set; }
        }

        public class SetDefaultAddressRequest
        {
            public SetDefaultAddressRequest() { }
            public string CustomerId { get; set; }
            public string AddressId { get; set; }
            public string Token { get; set; }
        }
        public class SetDefaultAddressResponse
        {
            public SetDefaultAddressResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string CustomerId { get; set; }
            public FullProfile Profile { get; set; }
        }

        public class GetAddressFromCoordinatesRequest
        {
            public GetAddressFromCoordinatesRequest() { }
            public string Latitude { get; set; }
            public string Longtitude { get; set; }
            public string PersonId { get; set; }
            public string ValidateVal { get; set; }
        }
        public class GetAddressFromCoordinatesResponse
        {
            public GetAddressFromCoordinatesResponse() { }
            public string Code { get; set; }
            public string Remark { get; set; }
            public string FullAddress { get; set; }
            public string AddressOne { get; set; }
            public string AddressTwo { get; set; }
        }

        public class ValidateCustomerAddressDataRequest
        {
            public int PersonId { get; set; }
            public string ValidateVal { get; set; }
            public int AddressId { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string FullAddress { get; set; }
        }

        public class ValidateCustomerAddressDataResponse : ResponseBase
        {
            public ValidateCustomerAddressDataResponse() { }
            public ValidateCustomerAddressDataResponse(ValidateCustomerAddressDataRequest req)
            {
                CustomerId = req.PersonId;
                AddressId = req.AddressId;
                Latitude = req.Latitude;
                Longitude = req.Longitude;
                FullAddress = req.FullAddress;
            }
            //Same as the request but with the possibly corrected data.
            public int CustomerId { get; set; }
            public int AddressId { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string FullAddress { get; set; }
        }

        #endregion

        #region Redeem Item and Points Earn

        public class RedeemTierItem
        {
            public RedeemTierItem() { }
            public int Id { get; set; }
            public int RedeemTierId { get; set; }
            public string ItemId { get; set; }
            public int RedeemPoint { get; set; }
            public bool CanRedeem { get; set; }
            public int TierId { get; set; }
            public string ItemName { get; set; }
        }

        public class PointEarn
        {
            public PointEarn() { }

            public string CustomerId { get; set; }
            public string Code { get; set; }
            public string TierId { get; set; }
            public string TierCode { get; set; }
            public string TierLevel { get; set; }
            public string GainId { get; set; }
            public string EarnMethod { get; set; }
            public string EarnType { get; set; }
            public string EarnItem { get; set; }
            public string EarnValue { get; set; }
            public string OriTotalPoint { get; set; }
            public string AfterTotalPoint { get; set; }

        }

        #endregion

        public class RefreshPointsResponse
        {
            public int CustomerId { get; set; }
            public bool RefreshSuccess { get; set; }
            public string RefreshMessage { get; set; }
            public string ErrorMessage { get; set; }
        }

        #region Referral Check

        public class ReferralCheckRequest
        {
            public string ReferralCode { get; set; }
        }

        public class ReferralCheckResponse : ResponseBase
        {
            public string ReferrerName { get; set; }
        }

        #endregion
    }
}