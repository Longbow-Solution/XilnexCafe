using AnWRestAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Reward
    {
        //For Redemption enumerators, go to Loyalty class for full details.
        //In DB, it is called as "UserReward"
        //This serves as replacement for Loyalty.UserReward

        public class PromotionReward
        {
            #region Properties

            public int UserRewardId { get; set; }
            public string UserRewardCode { get; set; }
            public string DisplayName { get; set; }
            public string RewardType { get; set; }
            public string RewardItemType { get; set; }
            public string DisplayDescription { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int ExpiryDuration
            {
                get
                {
                    return (int)Math.Floor((EndTime - StartTime).TotalDays);
                }
            }
            public string ImageFile { get; set; }
            public int Qty { get; set; }
            public bool IsRedeemable { get; set; }
            public int RedeemCost { get; set; }
            public decimal AmountValue { get; set; }
            public decimal AmountPercent { get; set; }
            public string CategoryId { get; set; }
            public string ItemId { get; set; }
            public RewardDetail RewardDetail { get; set; }
            //public List<RewardDetail> RewardDetails { get; set; }
            public string TermsAndConditions { get; set; }
            public Flag RewardFlag { get; set; }
            //public List<Flag> Flags { get; set; }
            public bool HasLimitByBranch 
            {
                get
                {
                    return BranchLimits == null ? false : BranchLimits.Count > 0;
                }
            }
            public List<LimitByBranch> BranchLimits { get; set; }
            public bool HasLimitByOrderType
            {
                get
                {
                    return OrderTypeLimits == null ? false : OrderTypeLimits.Count > 0;
                }
            }
            public List<LimitByOrderType> OrderTypeLimits { get; set; }
            public bool HasLimitByTier
            {
                get
                {
                    return TierLimits == null ? false : TierLimits.Count > 0;
                }
            }
            public List<LimitByTier> TierLimits { get; set; }
            public bool HasLimitByMenu
            {
                get
                {
                    return MenuLimits == null ? false : MenuLimits.Count > 0;
                }
            }
            public List<LimitByMenu> MenuLimits { get; set; }
            public bool HasLimitBySchedule
            {
                get
                {
                    return ScheduleLimits == null ? false : ScheduleLimits.Count > 0;
                }
            }
            public List<LimitBySchedule> ScheduleLimits { get; set; }
            public int Sequence { get; set; }
            public bool IsActive { get; set; }
            public string Status { get; set; }
            public string Remarks { get; set; }

            #endregion

            #region Constructor

            public PromotionReward()
            {
                CheckListInit();
            }

            #endregion

            #region Methods

            #region Helper Functions

            private bool IsValidForThatDay(LimitBySchedule scheduleLimit, DayOfWeek givenDoW)
            {
                switch (givenDoW) 
                {
                    case DayOfWeek.Sunday:
                        return scheduleLimit.IsSunday;
                    case DayOfWeek.Saturday:
                        return scheduleLimit.IsSaturday;
                    case DayOfWeek.Monday:
                        return scheduleLimit.IsMonday;
                    case DayOfWeek.Tuesday:
                        return scheduleLimit.IsTuesday;
                    case DayOfWeek.Wednesday:
                        return scheduleLimit.IsWednesday;
                    case DayOfWeek.Thursday:
                        return scheduleLimit.IsThursday;
                    case DayOfWeek.Friday:
                        return scheduleLimit.IsFriday;
                    default:
                        return false;
                }
            }

            #endregion

            #region Data Check

            private void CheckListInit()
            {
                //if (RewardDetails == null)
                //    RewardDetails = new List<RewardDetail>();
                //if (Flags == null)
                //    Flags = new List<Flag>();
                if (RewardDetail == null)
                    RewardDetail = new RewardDetail();
                if (RewardFlag == null)
                    RewardFlag = new Flag();
                if (BranchLimits == null)
                    BranchLimits = new List<LimitByBranch>();
                if (OrderTypeLimits == null)
                    OrderTypeLimits = new List<LimitByOrderType>();
                if (TierLimits == null)
                    TierLimits = new List<LimitByTier>();
                if (MenuLimits == null)
                    MenuLimits = new List<LimitByMenu>();
                if (ScheduleLimits == null)
                    ScheduleLimits = new List<LimitBySchedule>();
            }

            #endregion

            #region Type Check

            public Loyalty.RedemptionType GetRedemptionType()
            {
                if (!string.IsNullOrEmpty(RewardType))
                {
                    if (Enum.TryParse(RewardType, out Loyalty.RedemptionType parsedVal))
                    {
                        return parsedVal;
                    }
                    else
                    {
                        return Loyalty.RedemptionType.Unknown;
                    }
                }
                else
                {
                    return Loyalty.RedemptionType.Unknown;
                }
            }

            public Loyalty.RedemptionItemType GetRedemptionItemType()
            {
                if (!string.IsNullOrEmpty(RewardItemType))
                {
                    if (Enum.TryParse(RewardItemType, out Loyalty.RedemptionItemType parsedVal))
                    {
                        return parsedVal;
                    }
                    else
                    {
                        return Loyalty.RedemptionItemType.Unknown;
                    }
                }
                else
                {
                    return Loyalty.RedemptionItemType.Unknown;
                }
            }

            /// <summary>
            /// Determine whether this is a delivery discount voucher.
            /// </summary>
            /// <returns></returns>
            public bool IsDeliveryVoucher()
            {
                switch (GetRedemptionType())
                {
                    case Loyalty.RedemptionType.FirstDeliveryVoucher:
                    case Loyalty.RedemptionType.DeliveryPercentVoucher:
                    case Loyalty.RedemptionType.DeliveryAmountVoucher:
                    case Loyalty.RedemptionType.DeliveryAmountPromo:
                    case Loyalty.RedemptionType.DeliveryPercentPromo:
                        return true;
                    case Loyalty.RedemptionType.CartVoucherAmount:
                    case Loyalty.RedemptionType.CartVoucherPercentage:
                    case Loyalty.RedemptionType.CartPromoAmount:
                    case Loyalty.RedemptionType.CartPromoPercent:
                    case Loyalty.RedemptionType.Unknown:
                    case Loyalty.RedemptionType.Redeem:
                    case Loyalty.RedemptionType.Voucher:
                    case Loyalty.RedemptionType.Promo:
                    case Loyalty.RedemptionType.EmployeeVoucher:
                    case Loyalty.RedemptionType.GiftVoucher:
                    case Loyalty.RedemptionType.PointVoucher:
                    case Loyalty.RedemptionType.ItemVoucher:
                    default:
                        return false;
                }
            }
            /// <summary>
            /// Determine whether this is an item voucher.
            /// </summary>
            /// <returns></returns>
            public bool IsItemVoucher()
            {
                if (GetRedemptionType() == Loyalty.RedemptionType.ItemVoucher
                    || (GetRedemptionType() == Loyalty.RedemptionType.Voucher && GetRedemptionItemType() == Loyalty.RedemptionItemType.Item))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsDiscountVoucher()
            {
                if (GetRedemptionType() == Loyalty.RedemptionType.Voucher
                    && (GetRedemptionItemType() == Loyalty.RedemptionItemType.Amount || GetRedemptionItemType() == Loyalty.RedemptionItemType.Percentage))
                {
                    return true;
                }
                else
                {
                    switch (GetRedemptionType())
                    {
                        case Loyalty.RedemptionType.Voucher:
                            if (GetRedemptionItemType() != Loyalty.RedemptionItemType.Item)
                                return true;
                            else
                                return false;
                        case Loyalty.RedemptionType.EmployeeVoucher:
                        case Loyalty.RedemptionType.FirstDeliveryVoucher:
                        case Loyalty.RedemptionType.DeliveryPercentVoucher:
                        case Loyalty.RedemptionType.DeliveryAmountVoucher:
                        case Loyalty.RedemptionType.CartVoucherAmount:
                        case Loyalty.RedemptionType.CartVoucherPercentage:
                            return true;
                        case Loyalty.RedemptionType.DeliveryAmountPromo:
                        case Loyalty.RedemptionType.DeliveryPercentPromo:
                        case Loyalty.RedemptionType.CartPromoAmount:
                        case Loyalty.RedemptionType.CartPromoPercent:
                        case Loyalty.RedemptionType.ItemVoucher:
                        case Loyalty.RedemptionType.GiftVoucher:
                        case Loyalty.RedemptionType.PointVoucher:
                        case Loyalty.RedemptionType.Unknown:
                        case Loyalty.RedemptionType.Redeem:
                        case Loyalty.RedemptionType.Promo:
                        default:
                            return false;
                    }
                }
            }
            /// <summary>
            /// Determine whether this is a voucher-based reward.
            /// </summary>
            /// <returns></returns>
            public bool IsVoucher()
            {
                switch (GetRedemptionType())
                {
                    case Loyalty.RedemptionType.Voucher:
                    case Loyalty.RedemptionType.EmployeeVoucher:
                    case Loyalty.RedemptionType.GiftVoucher:
                    case Loyalty.RedemptionType.PointVoucher:
                    case Loyalty.RedemptionType.FirstDeliveryVoucher:
                    case Loyalty.RedemptionType.DeliveryPercentVoucher:
                    case Loyalty.RedemptionType.DeliveryAmountVoucher:
                    case Loyalty.RedemptionType.CartVoucherAmount:
                    case Loyalty.RedemptionType.CartVoucherPercentage:
                    case Loyalty.RedemptionType.ItemVoucher:
                        return true;
                    case Loyalty.RedemptionType.DeliveryAmountPromo:
                    case Loyalty.RedemptionType.DeliveryPercentPromo:
                    case Loyalty.RedemptionType.CartPromoAmount:
                    case Loyalty.RedemptionType.CartPromoPercent:
                    case Loyalty.RedemptionType.Unknown:
                    case Loyalty.RedemptionType.Redeem:
                    case Loyalty.RedemptionType.Promo:
                    default:
                        return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsPromo()
            {
                return GetRedemptionType() == Loyalty.RedemptionType.Promo;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsDiscountType()
            {
                //return GetRedemptionItemType() == Loyalty.RedemptionItemType.Amount || GetRedemptionItemType() == Loyalty.RedemptionItemType.Percentage;
                switch (GetRedemptionItemType())
                {
                    case Loyalty.RedemptionItemType.Amount:
                    case Loyalty.RedemptionItemType.Percentage:
                    case Loyalty.RedemptionItemType.DeliveryAmount:
                    case Loyalty.RedemptionItemType.DeliveryPercentage:
                        return true;
                    case Loyalty.RedemptionItemType.TenderAmount:
                    case Loyalty.RedemptionItemType.TenderPercentage:
                    case Loyalty.RedemptionItemType.Point:
                    case Loyalty.RedemptionItemType.Item:
                    case Loyalty.RedemptionItemType.Unknown:
                    default:
                        return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsItemType()
            {
                return GetRedemptionItemType() == Loyalty.RedemptionItemType.Item;
            }

            #endregion

            #region Eligibility Check

            /// <summary>
            /// Determine whether this reward is valid for use in an order.
            /// </summary>
            /// <param name="branchId"></param>
            /// <param name="orderTypeId"></param>
            /// <param name="orderPickupTime"></param>
            /// <param name="addedVoucherList"></param>
            /// <param name="customerOrderCart"></param>
            /// <param name="custTierId"></param>
            /// <param name="InvalidReason"></param>
            /// <returns>true if valid to use. false if invalid to use, and InvalidReason will always return a message.</returns>
            //public bool IsThisRewardEligibleForUse(string branchId, int orderTypeId, DateTime orderPickupTime, 
            //    List<Order.OrderReward> addedVoucherList, List<Order.OrderMenuList> customerOrderCart, int custTierId,
            //    out string InvalidReason)
            //{
            //    bool result = false;
            //    InvalidReason = string.Empty;

            //    //Double check on null parameter values.
            //    if (addedVoucherList == null)
            //        addedVoucherList = new List<Order.OrderReward>();
            //    if (customerOrderCart == null)
            //        customerOrderCart = new List<Order.OrderMenuList>();

            //    //Check on reward details.
            //    if (IsVoucher() && addedVoucherList.Any(avl => avl.IsPromo()))
            //    {
            //        InvalidReason = $"The {DisplayName} cannot be used together with any promo code.";
            //    }
            //    else if (IsPromo() && addedVoucherList.Any(avl => avl.IsVoucher()))
            //    {
            //        InvalidReason = $"The {DisplayName} cannot be used together with any voucher.";
            //    }
            //    //Check on flag values first.
            //    if (RewardFlag != null)
            //    {
            //        if (RewardFlag.HasMinSpend)
            //        {
            //            Order.OrderRequest reqInst = new Order.OrderRequest()
            //            {
            //                vouchers = addedVoucherList,
            //                orderMenuList = customerOrderCart
            //            };
            //            reqInst.CalculatePayment();
            //            if (reqInst.subTotal < RewardFlag.MinSpendAmt)
            //            {
            //                InvalidReason = $"You must meet the minimum RM{RewardFlag.MinSpendAmt:F2} to use the {DisplayName}.";
            //            }
            //        }
            //        if (RewardFlag.CanMix)
            //        {
            //            foreach (var addedPromo in addedVoucherList)
            //            {
            //                if (!RewardFlag.MixList.Any(ml => ml.UserRewardId == addedPromo.rewardSuperId))
            //                {
            //                    InvalidReason = $"The {DisplayName} cannot be used together with the {addedPromo.rewardName}.";
            //                    break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (addedVoucherList.Count > 1)
            //            {
            //                InvalidReason = $"The {DisplayName} cannot be used together with any voucher or promo code.";
            //            }
            //        }
            //    }
            //    //And then check on the limit(s) set.
            //    if (HasLimitBySchedule)
            //    {
            //        foreach (var schedule in ScheduleLimits)
            //        {
            //            var currentDay = orderPickupTime.DayOfWeek;
            //            if (!IsValidForThatDay(schedule, currentDay))
            //            {
            //                InvalidReason = $"The {DisplayName} is not valid for use on {currentDay}.";
            //                break;
            //            }
            //            if (orderPickupTime.Date >= schedule.StartDate.Date && orderPickupTime.Date <= schedule.EndDate.Date)
            //            {
            //                DateTime startTime = orderPickupTime.Date + schedule.StartTime.TimeOfDay;
            //                DateTime endTime = orderPickupTime.Date + schedule.EndTime.TimeOfDay;
            //                if (!(orderPickupTime >= startTime && orderPickupTime < endTime))
            //                {
            //                    InvalidReason = $"The {DisplayName} is valid for use between {startTime:HH:mm} and {endTime:HH:mm}.";
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                InvalidReason = $"The {DisplayName} is valid for use between {schedule.StartDate:dd/MM/yyyy} and {schedule.EndDate:dd/MM/yyyy}.";
            //                break;
            //            }
            //        }
            //    }
            //    if (HasLimitByOrderType)
            //    {
            //        foreach (var orderTypeRow in OrderTypeLimits)
            //        {
            //            if (orderTypeId != orderTypeRow.OrderTypeId)
            //            {
            //                string orderTypeName = orderTypeId == 2 ? "Take-Away" : "Dine-In";
            //                InvalidReason = $"The {DisplayName} is not valid for {orderTypeName}.";
            //                break;
            //            }
            //        }
            //    }
            //    if (HasLimitByMenu)
            //    {
            //        foreach (var menuRow in MenuLimits)
            //        {
            //            if (customerOrderCart.Any(coc => coc.itemId == menuRow.ItemId))
            //            {
            //                var thatMenu = customerOrderCart.Find(coc => coc.itemId == menuRow.ItemId);
            //                if (thatMenu.quantity < menuRow.MinQty)
            //                {
            //                    InvalidReason = $"The {DisplayName} is only valid with minimum {menuRow.MinQty}x {thatMenu.itemName} in cart.";
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                InvalidReason = $"The {DisplayName} is valid for certain menu in cart only.";
            //                break;
            //            }
            //        }
            //    }
            //    if (HasLimitByBranch)
            //    {
            //        foreach (var branch in BranchLimits)
            //        {
            //            if (branch.BranchId != branchId)
            //            {
            //                InvalidReason = $"The {DisplayName} is not valid for the selected outlet.";
            //                break;
            //            }
            //        }
            //    }
            //    if (HasLimitByTier)
            //    {
            //        if (!TierLimits.Any(tl => tl.TierId == custTierId))
            //        {
            //            InvalidReason = $"The {DisplayName} is not valid for your current tier.";
            //        }
            //    }

            //    if (string.IsNullOrEmpty(InvalidReason))
            //        result = true;

            //    return result;
            //}

            #endregion

            #endregion
        }

        #region Subclasses

        public class RewardDetail
        {
            public int DetailId { get; set; }
            public string Type { get; set; }
            public string VoucherCode { get; set; }
            public string PromoCode { get; set; }
            public int ClaimLimitTotalAll { get; set; }
            public int ClaimLimitPerUser { get; set; }
            public bool IsVoucherType
            {
                get
                {
                    return string.IsNullOrEmpty(Type) ? false : Type.ToLower().Equals("voucher");
                }
            }
            public bool IsPromoCodeType
            {
                get
                {
                    return string.IsNullOrEmpty(Type) ? false : Type.ToLower().Equals("promocode");
                }
            }
        }

        public class Flag
        {
            public int FlagId { get; set; }
            public bool HasMinSpend { get; set; }
            public decimal MinSpendAmt { get; set; }
            public bool HasMaxDiscount { get; set; }
            public decimal MaxDiscountAmt { get; set; }
            public bool CanGroup { get; set; }
            public bool CanTransfer { get; set; }
            public bool CanAutoApply { get; set; }
            public bool CanMix { get; set; }
            public List<MixList> MixList { get; set; }
            public bool CanRefund { get; set; }
            public bool CanAutoAssign { get; set; }
            public List<AssignSchedule> AssignSchedules { get; set; }

            public Flag()
            {
                if (MixList == null)
                    MixList = new List<MixList>();
                if (AssignSchedules == null)
                    AssignSchedules = new List<AssignSchedule>();
            }
        }

        public class MixList
        {
            public int DetailId { get; set; }
            public int UserRewardId { get; set; }
            public string DisplayName { get; set; }
        }

        public class AssignSchedule
        {
            public int ScheduleId { get; set; }
            public string ScheduleName { get; set; }
            public string Interval { get; set; }
            public DateTime AssignDate { get; set; }
            public int QtyPerUser { get; set; }
        }

        public class LimitByBranch
        {
            public int LimitId { get; set; }
            public string BranchId { get; set; }
            public string BranchName { get; set; }
        }

        public class LimitByOrderType
        {
            public int LimitId { get; set; }
            public int OrderTypeId { get; set; }
            public string OrderTypeName { get; set; }
        }

        public class LimitByTier
        {
            public int LimitId { get; set; }
            public int TierId { get; set; }
        }

        public class LimitByMenu
        {
            public int LimitId { get; set; }
            public string CategoryId { get; set; }
            public string ItemId { get; set; }
            public int MinQty { get; set; }
            public string ItemName { get; set; }
        }

        public class LimitBySchedule
        {
            public int LimitId { get; set; }
            public string ScheduleName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsMonday { get; set; }
            public bool IsTuesday { get; set; }
            public bool IsWednesday { get; set; }
            public bool IsThursday { get; set; }
            public bool IsFriday { get; set; }
            public bool IsSaturday { get; set; }
            public bool IsSunday { get; set; }
        }

        #endregion

    }
}