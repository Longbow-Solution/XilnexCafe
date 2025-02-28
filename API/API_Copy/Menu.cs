using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnWRestAPI.Models
{
    public class Menu
    {
        public enum MenuStatus
        {
            Active = 'A',
            Inactive = 'I',
            OutOfStock = 'O'
        }

        public class OrderTypeResponse
        {
            public List<OrderTypeList> orderTypeList { get; set; }
        }

        public class OrderTypeList
        {
            public OrderTypeList()
            {
            }

            public string orderTypeId { get; set; }
            public string orderTypeName { get; set; }
            public string img { get; set; }
        }

        public class MenuRequestT
        {
            public string branchId;
            public string merchantId;
            public string Authorization;
        }

        public class MenuRequest
        {
            public int restId;
        }

        public class MenuResponse
        {
            public string message { get; set; }
        }

        public class MenuList
        {
            [JsonProperty("errno", Required = Newtonsoft.Json.Required.Default)]
            public string errno { get; set; }

            [JsonProperty("data", Required = Newtonsoft.Json.Required.Default)]
            public DataList dataList { get; set; }

            [JsonProperty("errmsg", Required = Newtonsoft.Json.Required.Default)]
            public string errmsg { get; set; }
        }

        public class MenuListT
        {
            [JsonProperty("merchantID", Required = Newtonsoft.Json.Required.Default)]
            public string merchantID { get; set; }

            [JsonProperty("partnerMerchantID", Required = Newtonsoft.Json.Required.Default)]
            public string partnerMerchantID { get; set; }

            [JsonProperty("currency", Required = Newtonsoft.Json.Required.Default)]
            public CurrencyList currencyList { get; set; }

            [JsonProperty("sections", Required = Newtonsoft.Json.Required.Default)]
            public List<DataTList> dataTList { get; set; }
        }

       

        public class CurrencyList
        {
            public CurrencyList()
            {
            }

            public string code { get; set; }
            public string symbol { get; set; }
            public int exponent { get; set; }
        }

        public class DataList
        {
            public DataList()
            {
            }

            public string currencyType { get; set; }
            public List<PromotionWeekTimeList> promotionWeekTimeList { get; set; }
            public string restaurantName { get; set; }
            public string serviceFree { get; set; }
            public string roundingFormat { get; set; }
            public List<PromotionInfoList> promotionInfoList { get; set; }
            public List<Category> category { get; set; }
            public List<PromotionDateInfoList> promotionDateInfoList { get; set; }
            
        }

        public class DataTList
        {
            public DataTList()
            {
            }

            public string id { get; set; }
            public string name { get; set; }
            public int sequence { get; set; }
            public ServiceHours serviceHours { get; set; }
            public List<Categories> categories { get; set; }

        }

        public class ServiceHours
        {
            public ServiceHours()
            {
            }
            public DayWeek mon { get; set; }
            public DayWeek tue { get; set; }
            public DayWeek wed { get; set; }
            public DayWeek thu { get; set; }
            public DayWeek fri { get; set; }
            public DayWeek sat { get; set; }
            public DayWeek sun { get; set; }
        }

        public class DayWeek
        {
            public DayWeek()
            {
            }

            public string openPeriodType { get; set; }
            public List<Periods> periods { get; set; }
        }

        public class Periods
        {
            public Periods()
            {
            }

            public string startTime { get; set; }
            public string endTime { get; set; }
        }

        public class PromotionWeekTimeList
        {
            public PromotionWeekTimeList()
            {
            }

            public int id { get; set; }
            public int promotionDateInfoId { get; set; }
            public int week { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public int isActive { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
        }

        public class PromotionInfoList
        {
            public PromotionInfoList()
            {
            }

            public int id { get; set; }
            public int type { get; set; }
            public string promotionName { get; set; }
            public int restaurantId { get; set; }
            public int isActive { get; set; }
            public int promotionWeight { get; set; }
            public decimal discountPrice { get; set; }
            public decimal discountPercentage { get; set; }
            public int freeNum { get; set; }
            public int freeItemId { get; set; }
            public string freeItemName { get; set; }
            public int itemMainCategoryId { get; set; }
            public int itemCategoryId { get; set; }
            public int itemId { get; set; }
            public int itemNum { get; set; }
            public string itemMainCategoryName { get; set; }
            public string itemCategoryName { get; set; }
            public string itemName { get; set; }
            public int secondItemMainCategoryId { get; set; }
            public int secondItemCategoryId { get; set; }
            public string secondItemId { get; set; }
            public int secondItemNum { get; set; }
            public string secondItemMainCategoryName { get; set; }
            public string secondItemCategoryName { get; set; }
            public string secondItemName { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
            public decimal basePrice { get; set; }
            public int guestNum { get; set; }
            public int promotionDateInfoId { get; set; }
            public string itemCategoryList { get; set; }
            public string itemDetailList { get; set; }
            public int companyId { get; set; }
            public int areaId { get; set; }
            public List<PromotionInfoItemList> promotionInfoItemList { get; set; }
            public List<PromotionInfoSubCategoryList> promotionInfoSubCategoryList { get; set; }
            public List<PromotionInfoSalesTypeList> promotionInfoSalesTypeList { get; set; }
        }

        public class PromotionInfoItemList
        {
            public PromotionInfoItemList()
            {
            }

            public int id { get; set; }
            public int promotionInfoId { get; set; }
            public int itemId { get; set; }
            public int isFreeItem { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
        }

        public class PromotionInfoSubCategoryList
        {
            public PromotionInfoSubCategoryList()
            {
            }

            public int id { get; set; }
            public int promotionInfoId { get; set; }
            public int subCategoryId { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
        }

        public class PromotionInfoSalesTypeList
        {
            public PromotionInfoSalesTypeList()
            {
            }

            public int id { get; set; }
            public int promotionInfoId { get; set; }
            public int salesTypeId { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
        }

        public class Category
        {
            public Category()
            {
            }

            public int catId { get; set; }
            public string name { get; set; }
            public int indexId { get; set; }
            public List<Goods> goods { get; set; }
        }

        public class PromotionDateInfoList
        {
            public PromotionDateInfoList()
            {
            }

            public int id { get; set; }
            public int type { get; set; }
            public string promotionDateName { get; set; }
            public int restaurantId { get; set; }
            public int isActive { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
            public int dayStart { get; set; }
            public int dayEnd { get; set; }
            public string timeStart { get; set; }
            public string timeEnd { get; set; }
            public string promoStart { get; set; }
            public string promoEnd { get; set; }
            public int companyId { get; set; }
            public int aredId { get; set; }
        }

        public class Goods
        {
            public Goods()
            {
            }

            public int itemId { get; set; }
            public int itemTemplateId { get; set; }
            public int itemCategoryId { get; set; }
            public int itemType { get; set; }
            public string name { get; set; }
            public string itemDesc { get; set; }
            public string img { get; set; }
            public decimal price { get; set; }
            public int hasModifier { get; set; }
            public string itemCode { get; set; }
            public int max { get; set; }
            public int indexId { get; set; }
            public List<SalesType> salesType { get; set; }
            public List<TaxCategories> taxCategories { get; set; }
            public List<Modifiertypes> modifiertypes { get; set; }
        }

        public class SalesType
        {
            public SalesType()
            {
            }

            public int is_employee { get; set; }
            public int salesTypeCode { get; set; }
            public int disable { get; set; }
            public decimal price { get; set; }
            public int salesTypeId { get; set; }
            public string name { get; set; }
            public int is_default { get; set; }
            public int tax_id { get; set; }
            public int isInclusiveTax { get; set; }
        }

        public class TaxCategories
        {
            public TaxCategories()
            {
            }

            public int id { get; set; }
            public string taxCategoryName { get; set; }
            public List<Taxs> taxs { get; set; }
        }

        public class Taxs
        {
            public Taxs()
            {
            }

            public int categoryId { get; set; }
            public int taxOn { get; set; }
            public int taxOnId { get; set; }
            public int index { get; set; }
            public int taxId { get; set; }
            public string taxName { get; set; }
            public decimal taxPercentage { get; set; }
            public int taxType { get; set; }
            public string cendType { get; set; }
        }

        public class Modifiertypes
        {
            public Modifiertypes()
            {
            }

            public string modifierId { get; set; }
            public string name { get; set; }
            public int type { get; set; }
            public int isSet { get; set; }
            public int minNumber { get; set; }
            public int maxNumber { get; set; }
            public int mustDefault { get; set; }
            public string status { get; set; }
            public List<Modifiers> modifiers { get; set; }
        }

        public class Modifiers
        {
            public Modifiers()
            {
            }

            public string modifierId { get; set; }
            public int type { get; set; }
            public decimal price { get; set; }
            public decimal priceWOTax { get; set; }
            public decimal tax { get; set; }
            public string name { get; set; }
            public string itemId { get; set; }
            public int isDefault { get; set; }
            public int isSet { get; set; }
            public int qty { get; set; }
            public string img { get; set; }
            public int mustDefault { get; set; }
            public string status { get; set; }
            public List<Modifiertypes> modifiertypes { get; set; }
        }

        public class Categories
        {
            public Categories()
            {
            }

            public string id { get; set; }
            public string name { get; set; }
            public int sequence { get; set; }
            public string availableStatus { get; set; }
            public List<Items> items { get; set; }
        }

        public class Items
        {
            public Items()
            {
            }

            public string id { get; set; }
            public string name { get; set; }
            public int sequence { get; set; }
            public string availableStatus { get; set; }
            public string description { get; set; }
            public int price { get; set; }
            public List<string> photos { get; set; }
            public List<ModifierGroups> modifierGroups { get; set; }
        }

        public class ModifierGroups
        {
            public ModifierGroups()
            {
            }

            public string id { get; set; }
            public string name { get; set; }
            public int sequence { get; set; }
            public string availableStatus { get; set; }
            public int selectionRangeMin { get; set; }
            public int selectionRangeMax { get; set; }
            public List<ModifiersT> modifiers { get; set; }
        }

        public class ModifiersT
        {
            public ModifiersT()
            {
            }

            public string id { get; set; }
            public string name { get; set; }
            public int sequence { get; set; }
            public string availableStatus { get; set; }
            public int price { get; set; }
        }

        public class BranchCatRequest
        {
            public string branchId;
            public string channelType;
            public DateTime orderDateTime;
        }

        public class BranchCatResponse
        {
            public List<BranchCatList> branchCatList { get; set; }
            public List<BranchMenuList> branchMenuList { get; set; }
        }

        public class BranchCatList
        {            
            public BranchCatList()
            {
            }

            public int groupId { get; set; }
            public string groupName { get; set; }
            public string img { get; set; }
            public string imgOff { get; set; }
        }

        public class BranchMenuList
        {
            public BranchMenuList()
            {
            }

            public int groupId { get; set; }
            public string catId { get; set; }
            public string catName { get; set; }
            public string itemId { get; set; }
            public string itemTemplateId { get; set; }
            public string itemCategoryId { get; set; }
            public string itemType { get; set; }
            public string itemName { get; set; }
            public string itemDesc { get; set; }
            public string img { get; set; }
            public decimal originalPrice { get; set; }
            public decimal price { get; set; }
            public decimal priceWOTax { get; set; }
            public decimal tax { get; set; }
            public int hasModifier { get; set; }
            public string itemCode { get; set; }
            public string status { get; set; }
            public bool hasStock { get; set; }
            
            public bool IsInactive
            {
                get
                {
                    return status == "I";
                }
            }

            public List<Modifiertypes> modifiertypes { get; set; }
            
            public MenuUpgrade menuSizing { get; set; }
        }

        public class MenuUpgrade
        {
            public bool canUpsize { get; set; }
            public bool canDownsize { get; set; }
            public string fromItemId { get; set; }
            public string toItemId { get; set; }
        }

        public class MenuItemPlaceholder
        {
            public int Type { get; set; }
            public string BranchId { get; set; }
            public int GroupId { get; set; }
            public string CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string ModifierId { get; set; }
            public string ModifierName { get; set; }
            public string ParentItemId { get; set; }
            public string ParentItemName { get; set; }
            public string ItemId { get; set; }
            public string ItemName { get; set; }
            public string CurrentStatus { get; set; }
            public string NewStatus { get; set; }

            public MenuItemPlaceholder()
            {
                if (Type == 0)
                    Type = 1; //Default to Menu Level
            }

            public bool IsMenuLevel()
            {
                return Type == 1;
            }
            public bool IsModifierGroupLevel()
            {
                return Type == 2;
            }
            public bool IsModifierLevel()
            {
                return Type == 3;
            }
        }

        public class MenuLink
        {
            public MenuLink() { }
            public int LinkId { get; set; }
            public string FromItemId { get; set; }
            public string FromCategoryId { get; set; }
            public string FromItemName { get; set; }
            public string ToItemId { get; set; }
            public string ToCategoryId { get; set; }
            public string ToItemName { get; set; }
        }
    }
}