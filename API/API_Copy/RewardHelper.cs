using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using AnWRestAPI.Models;
using ANWSSK;
using Newtonsoft.Json;

namespace AnWRestAPI.Models
{
    public class RewardHelper
    {
        #region Private Fields

        private static readonly RewardHelper _instance = new RewardHelper();
        private static readonly string _moduleName = "RewardHelper";

        private static readonly string _freeRbMinimumSpendCategoryId = "LBCat14";
        private static readonly string _freeRbMinimumSpendItemId = "140049";
        private static readonly decimal _freeRbMinimumSpendAmount = 35.00m;
        private static readonly int[] _freeRbMinimumSpendRedeemId = { 906, 907, 908, 1034, 1035, 1036 };

        #endregion

        #region Static Properties

        public static RewardHelper Instance { get { return _instance; } }

        #endregion

        #region Properties

        private DateTime _lastUpdated = DateTime.MinValue;
        public DateTime LastUpdated
        {
            get
            {
                return _lastUpdated;
            }
        }

        private List<Reward.PromotionReward> _promotionRewardList = new List<Reward.PromotionReward>();
        public List<Reward.PromotionReward> PromotionRewardList
        {
            get
            {
                return _promotionRewardList;
            }
        }

        protected string SourceListPath { get; set; }

        #endregion

        #region Constructor

        private RewardHelper() { }

        public static RewardHelper GetInstance() { return _instance; }

        #endregion

        #region Methods

        #region Conversion

        /// <summary>
        /// Convert Reward.PromotionReward object to Loyalty.RedemptionItem, in order to be used on app.
        /// </summary>
        /// <param name="userReward">Reward.PromotionReward object.</param>
        /// <param name="txId">UserRewardTxId value. Usually 0 if the promotion is a promo code.</param>
        /// <returns>Loyalty.RedemptionItem object.</returns>
        public Loyalty.RedemptionItem ConvertToRedemptionItem(Reward.PromotionReward userReward, int txId)
        {
            if (userReward == null)
            {
                return new Loyalty.RedemptionItem();
            }
            DateTime currentDt = DateTime.Now;
            Loyalty.RedemptionItem buffer = new Loyalty.RedemptionItem()
            {
                Type = userReward.RewardType,
                ItemType = userReward.RewardItemType,
                ItemName = userReward.DisplayName,
                RedeemId = userReward.UserRewardId.ToString(),
                RedeemTxId = txId.ToString(),
                ItemImg = userReward.ImageFile,
                ExpiredDate = userReward.EndTime,
                Qty = 1,
                IsVoucher = userReward.IsVoucher(),
                IsPromoCode = userReward.IsPromo()
            };
            switch (userReward.GetRedemptionItemType())
            {
                case Loyalty.RedemptionItemType.Item:
                    buffer.ItemId = userReward.ItemId;
                    buffer.CatId = userReward.CategoryId;
                    break;
                case Loyalty.RedemptionItemType.Percentage:
                    buffer.Amount = userReward.AmountPercent;
                    buffer.MaxAmount = userReward.RewardFlag.HasMaxDiscount ? userReward.RewardFlag.MaxDiscountAmt : 9999m;
                    break;
                case Loyalty.RedemptionItemType.Amount:
                    buffer.Amount = userReward.AmountValue;
                    break;
                case Loyalty.RedemptionItemType.Point:
                case Loyalty.RedemptionItemType.Unknown:
                default:
                    break;
            }
            if (userReward.GetRedemptionType() == Loyalty.RedemptionType.Voucher || userReward.IsVoucher())
            {
                //Do nothing.
            }
            else if (userReward.GetRedemptionType() == Loyalty.RedemptionType.Promo)
            {
                buffer.Code = userReward.RewardDetail.PromoCode;
            }
            //No need to check ItemImg.
            if (currentDt < userReward.EndTime)
            {
                buffer.IsAvailable = "1";
                buffer.HasExpired = false;
            }
            else
            {
                buffer.IsAvailable = "0";
                buffer.HasExpired = true;
            }
            return buffer;
        }
        /// <summary>
        /// Convert Loyalty.RedemptionItem object to Order.OrderReward.
        /// </summary>
        /// <param name="userReward">Loyalty.RedemptionItem object.</param>
        /// <param name="rewardMenu">Order.OrderMenuList menu item to be returned if the promotion is an item menu.</param>
        /// <returns>Order.OrderReward object.</returns>
        public Order.OrderReward ConvertToOrderReward(Loyalty.RedemptionItem userReward, out Order.OrderMenuList rewardMenu)
        {
            rewardMenu = null;
            if (userReward == null)
            {
                return new Order.OrderReward();
            }
            Order.OrderReward buffer = new Order.OrderReward()
            {
                rewardSuperId = Convert.ToInt32(userReward.RedeemId),
                rewardTxId = Convert.ToInt32(userReward.RedeemTxId),
                rewardType = userReward.Type,
                rewardItemType = userReward.ItemType,
                rewardName = userReward.ItemName,
                rewardCode = userReward.Code,
                qty = 1
            };
            switch (userReward.GetRedemptionItemType())
            {
                case Loyalty.RedemptionItemType.Item:
                    buffer.isItem = true;
                    buffer.itemTxId = 0;
                    buffer.itemId = userReward.ItemId;
                    buffer.itemCatId = userReward.CatId;
                    buffer.itemCode = userReward.ItemCode;
                    buffer.itemName = userReward.ItemName;
                    buffer.itemPrice = 0.00m;
                    buffer.itemTax = 0.00m;
                    rewardMenu = ConvertToMenuItem(userReward);
                    break;
                case Loyalty.RedemptionItemType.Percentage:
                    buffer.isDiscount = true;
                    buffer.amount = userReward.Amount;
                    buffer.percentage = Convert.ToDouble(userReward.Amount);
                    buffer.maxAmount = userReward.MaxAmount;
                    break;
                case Loyalty.RedemptionItemType.Amount:
                    buffer.isDiscount = true;
                    buffer.amount = userReward.Amount;
                    break;
                case Loyalty.RedemptionItemType.Point:
                case Loyalty.RedemptionItemType.Unknown:
                default:
                    break;
            }
            if (userReward.GetRedemptionType() == Loyalty.RedemptionType.Voucher || userReward.IsVoucher)
            {
                buffer.isVoucher = true;
            }
            else if (userReward.GetRedemptionType() == Loyalty.RedemptionType.Promo || userReward.IsPromoCode)
            {
                buffer.isPromoCode = true;
                buffer.promoCode = userReward.Code;
            }
            if (userReward.GetRedemptionType() == Loyalty.RedemptionType.EmployeeVoucher || userReward.GetRedemptionType() == Loyalty.RedemptionType.GiftVoucher)
            {
                buffer.uniqueCode = userReward.SerialCode;
                if (userReward.GetRedemptionType() == Loyalty.RedemptionType.EmployeeVoucher)
                {
                    buffer.rewardTxId = 0;
                }
            }
            return buffer;
        }
        /// <summary>
        /// Convert Loyalty.RedemptionItem object to Order.OrderMenuList, in order to be added on order cart.
        /// </summary>
        /// <param name="userReward">Loyalty.RedemptionItem object.</param>
        /// <returns>Order.OrderMenuList object.</returns>
        public Order.OrderMenuList ConvertToMenuItem(Loyalty.RedemptionItem userReward)
        {
            if (userReward == null)
            {
                return new Order.OrderMenuList();
            }
            if (userReward.GetRedemptionItemType() == Loyalty.RedemptionItemType.Item)
            {
                Order.OrderMenuList buffer = new Order.OrderMenuList()
                {
                    itemCategoryId = userReward.CatId,
                    itemId = userReward.ItemId,
                    itemName = userReward.ItemName,
                    itemCode = userReward.ItemCode,
                    img = userReward.ItemImg,
                    price = 0.0m,
                    priceWOTax = 0.0m,
                    tax = 0.0m,
                    quantity = 1,
                    redeemId = Convert.ToInt32(userReward.RedeemId),
                    redeemTxId = Convert.ToInt32(userReward.RedeemTxId),
                    isReward = true,
                    orderModifiertypes = new List<Order.OrderModifiertypes>()
                };
                return buffer;
            }
            else
            {
                return new Order.OrderMenuList();
            }
        }

        #endregion

        #endregion
    }
}
