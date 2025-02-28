using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace AnWRestAPI.Models
{
    public class Order
    {
        #region Order Request from Customer

        //Frontend side must follow this exact setup, both variable name and functions.
        /// <summary>
        /// Main customer order data for ordering.
        /// </summary>
        public class OrderRequest
        {
            public OrderRequest(int txId, int orderId, int clientId, string mobileNo, string branchId, int addressId, int channelType, DateTime orderDateTime, DateTime collectionDateTime, DateTime? deliveryDateTime, string carPlateNo, bool cutlery, string orderRemark, string paymentType, decimal subTotal, decimal tax, decimal serviceCharge, decimal deliveryFee, decimal deliveryDiscount, decimal discount, decimal total, decimal rounding, decimal paymentTotal, decimal tenderTotal, string entityCId, string ogi, string noteToRider, string displayNumber, int promoTxId, int voucherTxId, List<OrderMenuList> orderMenuList, OrderPayment payment, List<OrderReward> vouchers)
            {
                this.txId = txId;
                this.orderId = orderId;
                this.clientId = clientId;
                this.mobileNo = mobileNo;
                this.branchId = branchId;
                this.addressId = addressId;
                this.channelType = channelType;
                this.orderDateTime = orderDateTime;
                this.collectionDateTime = collectionDateTime;
                this.deliveryDateTime = deliveryDateTime;
                this.carPlateNo = carPlateNo;
                this.cutlery = cutlery;
                this.orderRemark = orderRemark;
                this.paymentType = paymentType;
                this.subTotal = subTotal;
                this.tax = tax;
                this.serviceCharge = serviceCharge;
                this.deliveryFee = deliveryFee;
                this.deliveryDiscount = deliveryDiscount;
                this.discount = discount;
                this.total = total;
                this.rounding = rounding;
                this.paymentTotal = paymentTotal;
                this.tenderTotal = tenderTotal;
                EntityCId = entityCId;
                Ogi = ogi;
                this.noteToRider = noteToRider;
                this.displayNumber = displayNumber;
                this.promoTxId = promoTxId;
                this.voucherTxId = voucherTxId;
                this.orderMenuList = orderMenuList;
                this.payment = payment;
                this.vouchers = vouchers;
            }

            public int txId { get; set; }
            public int orderId { get; set; }
            public int clientId { get; set; } //this
            public string mobileNo { get; set; } // this
            public string branchId { get; set; } // this
            public int addressId { get; set; } 
            public int channelType { get; set; } //this
            public DateTime orderDateTime { get; set; } //this
            public DateTime collectionDateTime { get; set; }
            public DateTime? deliveryDateTime { get; set; } //ignore
            public string carPlateNo { get; set; } //ignore
            public bool cutlery { get; set; }//ignore
            public string orderRemark { get; set; }//ignore
            public string paymentType { get; set; }//paymentcode (int)
            public decimal subTotal { get; set; }
            public decimal tax { get; set; }
            public decimal serviceCharge { get; set; } //Deprecated.
            public decimal deliveryFee { get; set; } 
            public decimal deliveryDiscount { get; set; } 
            public decimal discount { get; set; }
            public decimal total { get; set; }
            public decimal rounding { get; set; }
            public decimal paymentTotal { get; set; }
            public decimal tenderTotal { get; set; } //For totalling up gift voucher amount
            public string EntityCId { get; set; }//Customer Id
            public string Ogi { get; set; }//UserToken //ignore
            public string noteToRider { get; set; } //ignore
            public string displayNumber { get; set; } //api return

            public int promoTxId { get; set; } //Deprecated.
            public int voucherTxId { get; set; } //Deprecated.
            public List<OrderMenuList> orderMenuList { get; set; }
            public OrderPayment payment { get; set; }
            public List<OrderReward> vouchers { get; set; } //Both vouchers and promo codes are stored here, despite the name suggests.

            #region Functional Functions

            /// <summary>
            /// Reset the amount tied to the cart only.
            /// </summary>
            public void ResetAmountValue()
            {
                subTotal = 0;
                tax = 0;
                total = 0;
                rounding = 0;
                paymentTotal = 0;
            }
            /// <summary>
            /// Reset all amount including vouchers and delivery.
            /// </summary>
            public void ResetAllValue()
            {
                ResetAmountValue();
                serviceCharge = 0;
                deliveryFee = 0;
                discount = 0;
                deliveryDiscount = 0;
                tenderTotal = 0;
            }
            /// <summary>
            /// Calculate the amount of the discount, delivery discount, and tender value based on the 'vouchers' list.
            /// </summary>
            public void EvaluateRewardValue()
            {
                if (vouchers == null)
                {
                    vouchers = new List<OrderReward>();
                }
                if (vouchers.Count > 0)
                {
                    discount = 0;
                    deliveryDiscount = 0;
                    tenderTotal = 0;
                    foreach (var voucher in vouchers)
                    {
                        if (voucher.GetRedemptionType() == Loyalty.RedemptionType.Voucher || voucher.GetRedemptionType() == Loyalty.RedemptionType.Promo)
                        {
                            if (voucher.GetRedemptionItemType() == Loyalty.RedemptionItemType.Amount)
                            {
                                discount += voucher.amount * voucher.qty;
                            }
                            else if (voucher.GetRedemptionItemType() == Loyalty.RedemptionItemType.Percentage)
                            {
                                decimal discountAmt = subTotal * (decimal)voucher.percentage / 100;
                                //Once calculated, be sure to round up to 2 decimal places before adding up to the delivery discount.
                                discountAmt = Math.Round(discountAmt, 2);
                                if (voucher.maxAmount > 0 && discountAmt > voucher.maxAmount)
                                {
                                    discountAmt = voucher.maxAmount;
                                }
                                discount += discountAmt; //Percentage-based discount ignores quantity
                            }
                        }
                        //Loyalty.RedemptionType.Voucher and Loyalty.RedemptionType.ItemVoucher is usually a menu item voucher so we don't check from here.
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.EmployeeVoucher)
                        {
                            //For example: RM10 voucher value times 5 vouchers.
                            discount += voucher.amount * voucher.qty;
                        }
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.GiftVoucher)
                        {
                            //Same as employee voucher but it sets to tender total.
                            tenderTotal += voucher.amount * voucher.qty;
                        }
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.FirstDeliveryVoucher)
                        {
                            decimal discountAmt = subTotal * (decimal)voucher.percentage / 100;
                            //Once calculated, be sure to round up to 2 decimal places before adding up to the delivery discount.
                            discountAmt = Math.Round(discountAmt, 2);
                            //The discount cannot exceed RM10 as per requirement.
                            if (discountAmt > 10.00m)
                            {
                                discountAmt = 10.00m;
                            }
                            discount += discountAmt;
                        }
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.DeliveryPercentVoucher)
                        {
                            decimal discountAmt = deliveryFee * (decimal)voucher.percentage / 100;
                            //Once calculated, be sure to round up to 2 decimal places before adding up to the delivery discount.
                            discountAmt = Math.Round(discountAmt, 2);
                            if (voucher.maxAmount > 0 && discountAmt > voucher.maxAmount)
                            {
                                discountAmt = voucher.maxAmount;
                            }
                            deliveryDiscount += discountAmt; //Percentage-based discount ignores quantity
                        }
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.DeliveryAmountVoucher)
                        {
                            deliveryDiscount += voucher.amount * voucher.qty;
                        }
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.CartVoucherAmount)
                        {
                            //Same logic as EmployeeVoucher.
                            discount += voucher.amount * voucher.qty;
                        }
                        else if (voucher.GetRedemptionType() == Loyalty.RedemptionType.CartVoucherPercentage)
                        {
                            //Same logic as FirstDeliveryVoucher but no discount limit.
                            decimal discountAmt = subTotal * (decimal)voucher.percentage / 100;
                            //Once calculated, be sure to round up to 2 decimal places before adding up to the delivery discount.
                            discountAmt = Math.Round(discountAmt, 2);
                            if (voucher.maxAmount > 0 && discountAmt > voucher.maxAmount)
                            {
                                discountAmt = voucher.maxAmount;
                            }
                            discount += discountAmt; //Percentage-based discount ignores quantity
                        }
                    }
                }
            }

            //Frontend side must follow this exact steps when order calculation is involved.
            /// <summary>
            /// Calculate the payment amount to be paid based on the cart and vouchers given.
            /// </summary>
            public void CalculatePayment()
            {
                ResetAmountValue();
                foreach (var menu in orderMenuList)
                {
                    subTotal += menu.price * menu.quantity;
                    tax += menu.tax * menu.quantity;
                }
                EvaluateRewardValue();
                // total = subtotal + tax + serviceCharge - discount;
                total = subTotal + serviceCharge - discount;
                if (total < 0)
                {
                    total = 0;
                }
                rounding = AnWRestAPI.GeneralFunc.RoundCurrencyAmount(total) - total;
                decimal nettDeliveryFee = deliveryFee - deliveryDiscount;
                if (nettDeliveryFee < 0)
                {
                    nettDeliveryFee = 0;
                }
                paymentTotal = total + rounding + nettDeliveryFee - tenderTotal;
                if (paymentTotal < 0)
                {
                    paymentTotal = 0;
                }
            }

            #endregion

            #region Get Functions

            /// <summary>
            /// Get all menu redeem items.
            /// </summary>
            /// <returns></returns>
            public List<OrderReward> GetAllMenuRedeemRewards()
            {
                if (vouchers == null)
                {
                    vouchers = new List<OrderReward>();
                }
                return vouchers.FindAll(x => x.GetRedemptionType() == Loyalty.RedemptionType.Voucher && x.GetRedemptionItemType() == Loyalty.RedemptionItemType.Item);
                //return vouchers.FindAll(x => x.rewardType == "Voucher" && x.rewardItemType == "Item");
            }
            /// <summary>
            /// Get all Employee Meal Vouchers.
            /// </summary>
            /// <returns></returns>
            public List<OrderReward> GetAllEmployeeVouchers()
            {
                if (vouchers == null)
                {
                    vouchers = new List<OrderReward>();
                }
                return vouchers.FindAll(x => x.GetRedemptionType() == Loyalty.RedemptionType.EmployeeVoucher);
            }
            /// <summary>
            /// Get all Gift Vouchers.
            /// </summary>
            /// <returns></returns>
            public List<OrderReward> GetAllGiftVouchers()
            {
                if (vouchers == null)
                {
                    vouchers = new List<OrderReward>();
                }
                return vouchers.FindAll(x => x.GetRedemptionType() == Loyalty.RedemptionType.GiftVoucher);
            }
            /// <summary>
            /// Is there a Delivery voucher in the vouchers list?
            /// </summary>
            /// <returns></returns>
            public bool HasDeliveryVoucher()
            {
                if (vouchers == null)
                {
                    vouchers = new List<OrderReward>();
                }
                return vouchers.Any(x => x.IsDeliveryVoucher() == true);
            }
            /// <summary>
            /// Get all Delivery vouchers.
            /// </summary>
            /// <returns></returns>
            public List<OrderReward> GetAllDeliveryVouchers()
            {
                if (vouchers == null)
                {
                    vouchers = new List<OrderReward>();
                }
                return vouchers.FindAll(x => x.IsDeliveryVoucher() == true);
            }

            public override bool Equals(object obj)
            {
                //return obj is OrderRequest request &&
                //       clientId == request.clientId &&
                //       mobileNo == request.mobileNo &&
                //       branchId == request.branchId &&
                //       //orderDateTime.Date == request.orderDateTime.Date &&
                //       channelType == request.channelType &&
                //       subTotal == request.subTotal &&
                //       tax == request.tax &&
                //       serviceCharge == request.serviceCharge &&
                //       deliveryFee == request.deliveryFee &&
                //       deliveryDiscount == request.deliveryDiscount &&
                //       discount == request.discount &&
                //       total == request.total &&
                //       rounding == request.rounding &&
                //       paymentTotal == request.paymentTotal &&
                //       tenderTotal == request.tenderTotal &&
                //       EntityCId == request.EntityCId &&
                //       EqualityComparer<List<OrderMenuList>>.Default.Equals(orderMenuList, request.orderMenuList) &&
                //       EqualityComparer<List<OrderReward>>.Default.Equals(vouchers, request.vouchers);
                return obj is OrderRequest request &&
                       clientId == request.clientId &&
                       mobileNo == request.mobileNo &&
                       branchId == request.branchId &&
                       //orderDateTime.Date == request.orderDateTime.Date &&
                       channelType == request.channelType &&
                       subTotal == request.subTotal &&
                       tax == request.tax &&
                       serviceCharge == request.serviceCharge &&
                       deliveryFee == request.deliveryFee &&
                       deliveryDiscount == request.deliveryDiscount &&
                       discount == request.discount &&
                       total == request.total &&
                       rounding == request.rounding &&
                       paymentTotal == request.paymentTotal &&
                       tenderTotal == request.tenderTotal &&
                       EntityCId == request.EntityCId &&
                       orderMenuList.All(oml1 => request.orderMenuList.Any(oml2 => oml1.itemId == oml2.itemId)) &&
                       vouchers.All(vl1 => request.vouchers.Any(vl2 => vl1.rewardSuperId == vl2.rewardSuperId && vl1.rewardTxId == vl2.rewardTxId));
            }

            public override int GetHashCode()
            {
                int hashCode = 219578039;
                hashCode = hashCode * -1521134295 + clientId.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(mobileNo);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(branchId);
                //hashCode = hashCode * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(orderDateTime.Date);
                hashCode = hashCode * -1521134295 + channelType.GetHashCode();
                hashCode = hashCode * -1521134295 + subTotal.GetHashCode();
                hashCode = hashCode * -1521134295 + tax.GetHashCode();
                hashCode = hashCode * -1521134295 + serviceCharge.GetHashCode();
                hashCode = hashCode * -1521134295 + deliveryFee.GetHashCode();
                hashCode = hashCode * -1521134295 + deliveryDiscount.GetHashCode();
                hashCode = hashCode * -1521134295 + discount.GetHashCode();
                hashCode = hashCode * -1521134295 + total.GetHashCode();
                hashCode = hashCode * -1521134295 + rounding.GetHashCode();
                hashCode = hashCode * -1521134295 + paymentTotal.GetHashCode();
                hashCode = hashCode * -1521134295 + tenderTotal.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EntityCId);
                //hashCode = hashCode * -1521134295 + EqualityComparer<List<OrderMenuList>>.Default.GetHashCode(orderMenuList);
                foreach (var menu in orderMenuList)
                {
                    hashCode = hashCode * -1521134295 + menu.GetHashCode();
                }
                //hashCode = hashCode * -1521134295 + EqualityComparer<List<OrderReward>>.Default.GetHashCode(vouchers);
                foreach (var promo in vouchers)
                {
                    hashCode = hashCode * -1521134295 + promo.GetHashCode();
                }
                return hashCode;
            }

            #endregion
        }

        public class OrderPayment
        {
            //public OrderPayment()
            //{
            //}

            public OrderPayment(int paymentTypeId, string ccName, string ccNo, string ccExp, string ccCvv, string ccCountry, decimal paymentAmount, string remarks)
            {
                this.paymentTypeId = paymentTypeId;
                this.ccName = ccName;
                this.ccNo = ccNo;
                this.ccExp = ccExp;
                this.ccCvv = ccCvv;
                this.ccCountry = ccCountry;
                this.paymentAmount = paymentAmount;
                this.remarks = remarks;
            }

            public int paymentTypeId { get; set; }
            public string ccName { get; set; }
            public string ccNo { get; set; }
            public string ccExp { get; set; }
            public string ccCvv { get; set; }
            public string ccCountry { get; set; }
            public decimal paymentAmount { get; set; }
            public string remarks { get; set; }
        }

        public class OrderMenuList
        {
            //public OrderMenuList()
            //{
            //}

            public OrderMenuList(string itemId, string itemTemplateId, string itemCategoryId, string itemType, string itemCode, string itemName, string itemDesc, string img, decimal originalPrice, decimal price, decimal priceWOTax, decimal tax, int quantity, int redeemId, int redeemTxId, bool isFixed, bool isReward, string upsellType, List<OrderModifiertypes> orderModifiertypes)
            {
                this.itemId = itemId;
                this.itemTemplateId = itemTemplateId;
                this.itemCategoryId = itemCategoryId;
                this.itemType = itemType;
                this.itemCode = itemCode;
                this.itemName = itemName;
                this.itemDesc = itemDesc;
                this.img = img;
                this.originalPrice = originalPrice;
                this.price = price;
                this.priceWOTax = priceWOTax;
                this.tax = tax;
                this.quantity = quantity;
                this.redeemId = redeemId;
                this.redeemTxId = redeemTxId;
                this.isFixed = isFixed;
                this.isReward = isReward;
                this.upsellType = upsellType;
                this.orderModifiertypes = orderModifiertypes;
            }

            public string itemId { get; set; }
            public string itemTemplateId { get; set; }
            public string itemCategoryId { get; set; }
            public string itemType { get; set; }
            public string itemCode { get; set; }
            public string itemName { get; set; }
            public string itemDesc { get; set; }
            public string img { get; set; }
            public decimal originalPrice { get; set; }
            public decimal price { get; set; }
            public decimal priceWOTax { get; set; }
            public decimal tax { get; set; }
            public int quantity { get; set; }
            public int redeemId { get; set; } //if itemvoucher
            public int redeemTxId { get; set; } //if itemvoucher
            public bool isFixed { get; set; } //cannot change, normally itemvoucher
            public bool isReward { get; set; }//if itemvoucher
            public string upsellType { get; set; } //NEW

            public List<OrderModifiertypes> orderModifiertypes { get; set; }

            public override bool Equals(object obj)
            {
                return obj is OrderMenuList list &&
                       itemId == list.itemId &&
                       itemCategoryId == list.itemCategoryId &&
                       itemType == list.itemType &&
                       itemCode == list.itemCode &&
                       itemName == list.itemName &&
                       originalPrice == list.originalPrice &&
                       price == list.price &&
                       priceWOTax == list.priceWOTax &&
                       tax == list.tax &&
                       quantity == list.quantity &&
                       EqualityComparer<List<OrderModifiertypes>>.Default.Equals(orderModifiertypes, list.orderModifiertypes);
            }

            public override int GetHashCode()
            {
                int hashCode = -664155409;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemId);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemCategoryId);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemType);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemCode);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemName);
                hashCode = hashCode * -1521134295 + originalPrice.GetHashCode();
                hashCode = hashCode * -1521134295 + price.GetHashCode();
                hashCode = hashCode * -1521134295 + priceWOTax.GetHashCode();
                hashCode = hashCode * -1521134295 + tax.GetHashCode();
                hashCode = hashCode * -1521134295 + quantity.GetHashCode();
                //hashCode = hashCode * -1521134295 + EqualityComparer<List<OrderModifiertypes>>.Default.GetHashCode(orderModifiertypes);
                foreach (var modifierGroup in orderModifiertypes)
                {
                    hashCode = hashCode * -1521134295 + modifierGroup.GetHashCode();
                }
                return hashCode;
            }
        }

        public class OrderModifiertypes
        {
            public OrderModifiertypes()
            {
            }

            public string modifierId { get; set; }
            public string name { get; set; }
            public int qty { get; set; }
            public decimal price { get; set; }
            public List<OrderModifiers> orderModifiers { get; set; }

            public override bool Equals(object obj)
            {
                return obj is OrderModifiertypes modifiertypes &&
                       modifierId == modifiertypes.modifierId &&
                       EqualityComparer<List<OrderModifiers>>.Default.Equals(orderModifiers, modifiertypes.orderModifiers);
            }

            public override int GetHashCode()
            {
                int hashCode = -295670260;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(modifierId);
                //hashCode = hashCode * -1521134295 + EqualityComparer<List<OrderModifiers>>.Default.GetHashCode(orderModifiers);
                foreach (var modifier in orderModifiers)
                {
                    hashCode = hashCode * -1521134295 + modifier.GetHashCode();
                }
                return hashCode;
            }
        }

        public class OrderModifiers
        {
            public OrderModifiers()
            {
            }

            public string modifierId { get; set; }
            public string name { get; set; }
            public int qty { get; set; }
            public decimal price { get; set; }
            public decimal priceWOTax { get; set; }
            public decimal tax { get; set; }
            public bool isFoc { get; set; }

            public override bool Equals(object obj)
            {
                return obj is OrderModifiers modifiers &&
                       modifierId == modifiers.modifierId &&
                       qty == modifiers.qty &&
                       price == modifiers.price &&
                       priceWOTax == modifiers.priceWOTax &&
                       tax == modifiers.tax;
            }

            public override int GetHashCode()
            {
                int hashCode = 411055353;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(modifierId);
                hashCode = hashCode * -1521134295 + qty.GetHashCode();
                hashCode = hashCode * -1521134295 + price.GetHashCode();
                hashCode = hashCode * -1521134295 + priceWOTax.GetHashCode();
                hashCode = hashCode * -1521134295 + tax.GetHashCode();
                return hashCode;
            }
        }
        /// <summary>
        /// Class to identify App Rewards which includes Discounts and Vouchers.
        /// </summary>
        public class OrderReward
        {
            public OrderReward() { }

            public int rewardTxId { get; set; }
            public int rewardSuperId { get; set; }
            //To check for valid reward type and reward item type values, refer to Loyalty.RedemptionType enum.
            public string rewardType { get; set; }
            public string rewardItemType { get; set; }
            public int qty { get; set; }
            public string rewardName { get; set; }
            public string rewardCode { get; set; }
            //For Promo Code based rewards.
            public string promoCode { get; set; }
            //For Employee Meal and Gift Vouchers
            public string uniqueCode { get; set; }
            //For 'Amount' rewardItemType
            public decimal amount { get; set; }
            //For 'Percentage' rewardItemType
            public double percentage { get; set; }
            public decimal maxAmount { get; set; }
            //For 'Item' rewardItemType 
            public int itemTxId { get; set; }
            public string itemCatId { get; set; }
            public string itemId { get; set; }
            public string itemCode { get; set; }
            public string itemName { get; set; }
            public decimal itemPrice { get; set; }
            public decimal itemTax { get; set; }
            public bool isVoucher { get; set; }
            public bool isPromoCode { get; set; }
            public bool isDiscount { get; set; } //This applies to tender-based and delivery-discount-based as well.
            public bool isItem { get; set; }
            /// <summary>
            /// Get the Redemption Type of the reward.
            /// </summary>
            /// <returns></returns>
            public Loyalty.RedemptionType GetRedemptionType()
            {
                if (!string.IsNullOrEmpty(rewardType))
                {
                    if (Enum.TryParse(rewardType, out Loyalty.RedemptionType parsedVal))
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
            /// <summary>
            /// Get the Redemption Item Type of the reward.
            /// </summary>
            /// <returns></returns>
            public Loyalty.RedemptionItemType GetRedemptionItemType()
            {
                if (!string.IsNullOrEmpty(rewardItemType))
                {
                    if (Enum.TryParse(rewardItemType, out Loyalty.RedemptionItemType parsedVal))
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
            /// <summary>
            /// 
            /// </summary>
            public void SetRewardBoolean()
            {
                isVoucher = IsVoucher();
                isPromoCode = IsPromo();
                isDiscount = IsDiscountType();
                isItem = IsItemType();
            }

            public override bool Equals(object obj)
            {
                return obj is OrderReward reward &&
                       rewardTxId == reward.rewardTxId &&
                       rewardSuperId == reward.rewardSuperId &&
                       rewardType == reward.rewardType &&
                       rewardItemType == reward.rewardItemType &&
                       rewardCode == reward.rewardCode &&
                       promoCode == reward.promoCode &&
                       uniqueCode == reward.uniqueCode;
            }

            public override int GetHashCode()
            {
                int hashCode = -1246734959;
                hashCode = hashCode * -1521134295 + rewardTxId.GetHashCode();
                hashCode = hashCode * -1521134295 + rewardSuperId.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rewardType);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rewardItemType);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rewardCode);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(promoCode);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(uniqueCode);
                return hashCode;
            }
        }

        #endregion

        public class InitialOrderResponse
        {
            public string orderNo { get; set; }
            public string shortOrderNumber { get; set; }
            public string errCode { get; set; }
            public string message { get; set; }
            public string TxSalesId { get; set; }

            public List<OrderItemToRemove> orderItemToRemoveList { get; set; }
        }

        public class OrderItemToRemove
        {
            public OrderItemToRemove()
            {
            }

            public string itemId { get; set; }
            public string itemName { get; set; }
        }

        public class OrderResponse
        {
            public string orderNo { get; set; }
            public string shortOrderNumber { get; set; }
            public int earnPoints { get; set; }
            public int currentPoints { get; set; }
            public string message { get; set; }
        }

        public class OrderRequestT
        {
            public string orderID { get; set; }
            public string merchantID { get; set; }
            public string partnerMerchantId { get; set; }
            public DateTime orderTime { get; set; }
            public string paymentType { get; set; }
            public string shortOrderNumber { get; set; }
            public CurrencyT currency { get; set; }
            public FeataureFlags featureFlags { get; set; }
            public bool cutlery { get; set; }
            public List<OrderItemsListT> items { get; set; }
            public OrderPriceT price { get; set; }
        }

        public class OrderResponseT
        {
            public string target { get; set; }
            public string reason { get; set; }
            public string message { get; set; }
        }

        public class CurrencyT
        {
            public CurrencyT()
            {
            }

            public string code { get; set; }
            public string symbol { get; set; }
            public int exponent { get; set; }
        }

        public class OrderItemsListT
        {
            public OrderItemsListT()
            {
            }

            public string id { get; set; }
            public int quantity { get; set; }
            public string specifications { get; set; }
            public int price { get; set; }
            public List<OrderItemsModifiersT> modifiers { get; set; }
        }

        public class OrderItemsModifiersT
        {
            public OrderItemsModifiersT()
            {
            }

            public string id { get; set; }
            public int price { get; set; }
            public int quantity { get; set; }
        }

        public class OrderPriceT
        {
            public OrderPriceT()
            {
            }

            public int subtotal { get; set; }
            public int tax { get; set; }
            public int deliveryFee { get; set; }
            public int eaterPayment { get; set; }
            public int FundPromo { get; set; }
            public int merchantFundPromo { get; set; }
        }

        public class FeataureFlags
        {
            public FeataureFlags()
            {               
            }

            public string orderAcceptedType { get; set; }
        }

        public class OrderRequest360
        {
            public int bizType { get; set; }
            public string currencyCode { get; set; }
            public string currencyType { get; set; }
            public int devType { get; set; }
            public int eatType { get; set; }
            public decimal grabAmount { get; set; }
            public List<OrderItemList360> itemList { get; set; }
            public string languageCode { get; set; }
            public string mobile { get; set; }
            public string orderRemark { get; set; }
            public int people { get; set; }
            public int restaurantId { get; set; }
            public string restaurantName { get; set; }
            public decimal serviceFree { get; set; }
            public int sourceType { get; set; }
            public decimal subTotal { get; set; }
            public string tableNo { get; set; }
            public decimal taxAmount { get; set; }
            public string token { get; set; }
            public decimal total { get; set; }
            public decimal totalOnServiceFree { get; set; }
            public int userId { get; set; }
        }

        public class OrderItemList360
        {
            public OrderItemList360()
            {
            }

            public int hasModifier { get; set; }
            public string img { get; set; }
            public string itemCode { get; set; }
            public string itemDesc { get; set; }
            public int itemId { get; set; }
            public int itemNum { get; set; }
            public decimal itemPrice { get; set; }
            public int itemTemplateId { get; set; }
            public int itemType { get; set; }
            public List<Modifiers360> modifiers { get; set; }
            public string name { get; set; }
            public decimal subTotalPrice { get; set; }
            public List<OrderTaxs360> taxs { get; set; }
            public decimal totalPrice { get; set; }
        }

        public class OrderTaxs360
        {
            public OrderTaxs360()
            {
            }

            public int taxCategoryId { get; set; }
            public int taxId { get; set; }
            public int taxOn { get; set; }
            public int taxOnId { get; set; }
            public decimal taxPercentage { get; set; }
            public decimal taxPrice { get; set; }
            public int taxType { get; set; }
        }

        public class Modifiers360
        {
            public Modifiers360()
            {
            }

            public int modifierId { get; set; }
            public int modifierNum { get; set; }
            public decimal modifierPrice { get; set; }
            public string name { get; set; }
            public int modifierParentId { get; set; }
        }

        public class OrderResponse360
        {
            public string errno { get; set; }
            public Data360 data { get; set; }
            public string errmsg { get; set; }
        }

        public class Data360
        {
            public Data360()
            {
            }

            public string barCode { get; set; }
            public string printNumber { get; set; }
            public string orderId { get; set; }
            public string orderNum { get; set; }
            public string userId { get; set; }
            public string orderStatus { get; set; }
            public string restaurantId { get; set; }
            public string restaurantName { get; set; }
            public string orderAddress { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public int manageShopId { get; set; }
            public int person { get; set; }
            public string orderDesc { get; set; }
            public int itemCount { get; set; }
            public string orderRemark { get; set; }
            public string placeOrderTime { get; set; }
            public DateTime date { get; set; }
            public int payStatus { get; set; }
            public int bizType { get; set; }
            public int sourceType { get; set; }
            public int payType { get; set; }
            public int thirdType { get; set; }
            public string tableNo { get; set; }
            public string outerOrderId { get; set; }
            public decimal taxAmount { get; set; }
            public decimal total { get; set; }
            public string currencyType { get; set; }
            public decimal totalOnServiceFree { get; set; }
            public decimal realPaymentAmount { get; set; }
            public string deliveryTime { get; set; }
            public string contact { get; set; }
            public string mobile { get; set; }
            public int diningStatus { get; set; }
            public decimal grabAmount { get; set; }
            public string deliveryID { get; set; }
            public int eatType { get; set; }
            public decimal paymentAmount { get; set; }
            public decimal payBackAmount { get; set; }
            public decimal notPaymentAmount { get; set; }
            public int companyId { get; set; }
            public List<OrderDetail360> orderDetailList { get; set; }
            public List<OrderTax360> orderTaxList { get; set; }
        }

        public class OrderDetail360
        {
            public OrderDetail360()
            {
            }
            public int orderDetailId { get; set; }
            public int itemId { get; set; }
            public int categoryId { get; set; }
            public int itemNum { get; set; }
            public string itemCode { get; set; }
            public string itemName { get; set; }
            public int itemType { get; set; }
            public int isMarketPrice { get; set; }
            public int minChoose { get; set; }
            public string unit { get; set; }
            public string img { get; set; }
            public decimal itemPrice { get; set; }
            public decimal totalItemPrice { get; set; }
            public decimal discountPrice { get; set; }
            public decimal realPrice { get; set; }
            public decimal modifierPrice { get; set; }
            public List<ModifierTypes360> modifiertypes { get; set; }
            public List<Taxs360> taxs { get; set; }
        }

        public class ModifierTypes360
        {
            public ModifierTypes360()
            {
            }

            public int orderDetailId { get; set; }
            public int modifierTypeId { get; set; }
            public string modifierTypeName { get; set; }
            public int isSet { get; set; }
        }

        public class Modifier360
        {
            public Modifier360()
            {
            }

            public int modifierId { get; set; }
            public string modifierItemCode { get; set; }
            public string modifierName { get; set; }
            public int modifierNum { get; set; }
            public decimal modifierPrice { get; set; }
            public int itemId { get; set; }
        }

        public class Taxs360
        {
            public Taxs360()
            {
            }

            public int taxId { get; set; }
            public int taxType { get; set; }
            public string taxName { get; set; }
            public decimal taxPrice { get; set; }
        }

        public class OrderTax360
        {
            public OrderTax360()
            {
            }

            public string taxName { get; set; }
            public decimal taxPrice { get; set; }
        }
        
        //public class PaymentRequest360
        //{
        //    public int userId;
        //    public int orderID;
        //    public int bizType;
        //    public int sourceType;
        //    public int payType;
        //    public int bizOrderNo;
        //    public string tradeNo;
        //    public decimal amount;
        //    public string buyerUsername;
        //    public decimal payPrice;
        //    public string tradeTime;
        //    public string sysPayId;
        //    public int companyId;
        //    public string token;
        //}

        public class PaymentResponse360
        {
            public int errno { get; set; } 
            public PaymentData data { get; set; }
            public string errmsg { get; set; }
        }

        public class PaymentData
        {
            public PaymentData()
            {

            }

            public int userId { get; set; }
            public int sourceType { get; set; }
            public int devType { get; set; }
            public string deviceId { get; set; }
            public string barCode { get; set; }
            public string printNumber { get; set; }
            public int orderId { get; set; }
            public int orderStatus { get; set; }
            public int diningStatus { get; set; }
            public int restaurantId { get; set; }
            public string restaurantName { get; set; }
            public int manageShopId { get; set; }
            public int person { get; set; }
            public string orderDesc { get; set; }
            public int itemCount { get; set; }
            public DateTime placeOrderTime { get; set; }
            public int payStatus { get; set; }
            public decimal total { get; set; }
            public int bizType { get; set; }
            public string outerOrderId { get; set; }
            public decimal discountAmount { get; set; }
            public string subsidiesId { get; set; }
            public decimal sidyAmount { get; set; }
            public decimal subsidy { get; set; }
            public decimal companySubsidy { get; set; }
            public decimal memberDiscountAmount { get; set; }
            public decimal couponAmount { get; set; }
            public decimal privilegeDiscount { get; set; }
            public decimal paymentAmount { get; set; }
            public decimal payBackAmount { get; set; }
            public decimal notPaymentAmount { get; set; }
            public string tableNo { get; set; }
            public DateTime payTime { get; set; }
            public string empName { get; set; }
            public string memberNo { get; set; }
            public PaymentList paymentList { get; set; }
            public int paypalToken { get; set; }
            public DateTime businessDueAmount { get; set; }
        }

        public class PaymentList
        {
            public PaymentList()
            {

            }
        }

        public class PaymentTypeResponse
        {
            public List<PaymentTypeList> paymentTypeList { get; set; }
        }

        public class PaymentTypeList
        {
            public string paymentGroupCode { get; set; }
            public string paymentGroupName { get; set; }
            public List<PaymentTypeDetList> paymentTypeDetList { get; set; }
        }

        public class PaymentTypeDetList
        {
            public string paymentCode { get; set; }
            public string paymentName { get; set; }
            public string image { get; set; }
            public string ipayId { get; set; }
        }

        public class OrderHistoryResponse
        {
            public List<OrderHistoryList> orderHistoryList { get; set; }
        }

        public class OrderHistoryList
        {
            public OrderHistoryList()
            {
            }

            public string TxId { get; set; }
            public string ShortOrderNo { get; set; }
            public string BranchName { get; set; }
            public string TxDate { get; set; }
            public string TxAmt { get; set; }
            public int EarnPoint { get; set; }
            public int RedeemPoints { get; set; }
            public string Status { get; set; }
            public string OrderTypeName { get; set; }
        }

        public class OrderDetailHistoryResponse
        {
            public OrderDetailHistory orderDetailHistory { get; set; }
        }

        public class OrderDetailHistory
        {
            public OrderDetailHistory()
            {
            }

            public string shortOrderNo;
            public string branchName;
            public int channelType;
            public string channelTypeName;
            public DateTime orderDateTime;
            public DateTime collectionDateTime;
            public bool cutlery;
            public string orderRemark;
            public string paymentType;
            public decimal subTotal;
            public decimal tax;
            public decimal serviceCharge;
            public decimal discount;
            public decimal deliveryFee;
            public decimal deliveryDiscount;
            public decimal total;
            public decimal rounding;
            public decimal paymentTotal;
            public decimal tenderTotal;
            public int earnPoint;

            public List<OrderMenuHistoryList> orderMenuList { get; set; }
            public Delivery.DeliveryOrderItem deliveryDetails { get; set; }
        }

        public class OrderMenuHistoryList
        {
            public OrderMenuHistoryList()
            {
            }

            public string itemId { get; set; }
            public string itemName { get; set; }
            public string itemDesc { get; set; }
            public decimal price { get; set; }
            public decimal priceWOTax { get; set; }
            public decimal tax { get; set; }
            public int quantity { get; set; }

            public List<OrderModifiers> orderModifiers { get; set; }
        }

        public class OrderCallbackResponse
        {
            public OrderCallbackResponse() { }

            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
        }

        public class RadiantMenuProp
        {
            public RadiantMenuProp() { }

            public int TrackId { get; set; }
            public string BranchId { get; set; }
            public string CategoryId { get; set; }
            public string MenuId { get; set; }
            public string ModifierGroupId { get; set; }
            public bool IsComboParent { get; set; }
            public bool IsCondi { get; set; }
            public bool IsMealDeal { get; set; }
            public string ModifierGroupParentId { get; set; }
        }

        public class DeliveryEstimateRequest
        {
            public DeliveryEstimateRequest() { }

            public string branchId { get; set; }
            public int addressId { get; set; }
            public int customerId { get; set; }
        }

        public class DeliveryEstimateResponse
        {
            public DeliveryEstimateResponse() { }

            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public decimal DeliveryFee { get; set; }
            public string DeliveryTime { get; set; }
        }

        public class SendNotifyRequest
        {
            public SendNotifyRequest() { }

            public int CustomerId { get; set; }
            public string Message { get; set; }
        }

        public class CartPromotion
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string DisplayName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int Qty { get; set; }
            public string ItemId { get; set; }
            public string CatId { get; set; }
            public decimal MinimumSpendAmt { get; set; }
            public int RemainingQty { get; set; }
            /// <summary>
            /// This holds the branch IDs, not branch names.
            /// </summary>
            public List<string> Branches { get; set; }
            /// <summary>
            /// Parse the column data from SqlDataReader to the respective properties.
            /// </summary>
            /// <param name="readRow"></param>
            public void ParseReaderToProps(System.Data.SqlClient.SqlDataReader readRow)
            {
                if (readRow["Id"] != null)
                {
                    Id = AnWRestAPI.GeneralFunc.TryGetIntValue(readRow["Id"]);
                }
                if (readRow["Code"] != null)
                {
                    Code = AnWRestAPI.GeneralFunc.TryGetToString(readRow["Code"]);
                }
                if (readRow["DisplayName"] != null)
                {
                    DisplayName = AnWRestAPI.GeneralFunc.TryGetToString(readRow["DisplayName"]);
                }
                if (readRow["StartTime"] != null)
                {
                    if (DateTime.TryParse(AnWRestAPI.GeneralFunc.TryGetToString(readRow["StartTime"]), out DateTime parsedDt))
                    {
                        StartTime = parsedDt;
                    }
                }
                if (readRow["EndTime"] != null)
                {
                    if (DateTime.TryParse(AnWRestAPI.GeneralFunc.TryGetToString(readRow["EndTime"]), out DateTime parsedDt))
                    {
                        EndTime = parsedDt;
                    }
                }
                if (readRow["Qty"] != null)
                {
                    Qty = AnWRestAPI.GeneralFunc.TryGetIntValue(readRow["Qty"]);
                }
                if (readRow["ItemId"] != null)
                {
                    ItemId = AnWRestAPI.GeneralFunc.TryGetToString(readRow["ItemId"]);
                }
                if (readRow["CatId"] != null)
                {
                    CatId = AnWRestAPI.GeneralFunc.TryGetToString(readRow["CatId"]);
                }
                if (readRow["MinimumSpendAmt"] != null)
                {
                    MinimumSpendAmt = AnWRestAPI.GeneralFunc.TryGetDecimalValue(readRow["MinimumSpendAmt"]);
                }
                if (readRow["Branches"] != null)
                {
                    string fullStr = AnWRestAPI.GeneralFunc.TryGetToString(readRow["Branches"]);
                    if (!string.IsNullOrWhiteSpace(fullStr))
                    {
                        Branches = fullStr.Split(',').ToList();
                    }
                }
                if (readRow["QtyRemarks"] != null)
                {
                    //The QtyRemarks is a string value, so it will contain something like this: 'Remaining:100'.
                    string fullStr = AnWRestAPI.GeneralFunc.TryGetToString(readRow["QtyRemarks"]);
                    string[] splitStr = fullStr.Split(':');
                    if (splitStr.Length == 2)
                    {
                        RemainingQty = int.Parse(splitStr[1]);
                    }
                }
            }

            public string GetRemainingValueString()
            {
                return $"Remaining:{RemainingQty}";
            }
        }

        public class CheckCartPromoRequest
        {
            public string OutletName { get; set; }
            public OrderRequest OrderObj { get; set; }
        }

        public class CheckCartPromoResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public List<OrderMenuList> EligibleItems { get; set; }
            public List<OrderMenuList> InvalidItems { get; set; }
        }

        public class ManualOrderSendRequest
        {
            public int OrderNumber { get; set; }
            public string DisplayNumber { get; set; }
            public string BranchId { get; set; }
            public decimal PaidAmount { get; set; }
        }
        
        public class ManualOrderSendResponse
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
        }

        public class VerificationOrderDataRequest
        {
            public int CustomerId { get; set; }
            public string ValidateString { get; set; } //This is actually user token, just different prop name for security purpose.
            public OrderRequest OrderData { get; set; }
        }

        //public class VerificationOrderDataResponse : ResponseBase
        //{
        //    public int CustomerId { get; set; }
        //    public OrderRequest OrderData { get; set; }

        //    public void ImportFromRequest(VerificationOrderDataRequest req)
        //    {
        //        CustomerId = req.CustomerId;
        //        OrderData = req.OrderData;
        //        if (OrderData == null)
        //            OrderData = new OrderRequest();
        //    }
        //}
    }
}