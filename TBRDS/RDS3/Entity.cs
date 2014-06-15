using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultra.Web.Core.Common;
using System.Xml.Serialization;

namespace RDS3
{
    class Entity
    {
    }

    /// <summary>
    /// T_ERP_TaoBao_Trade:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_ERP_TaoBao_Trade
    {
        public T_ERP_TaoBao_Trade()
        { }

        public List<T_ERP_TaoBao_Order> OrderList { get; set; }
        public List<T_ERP_TaoBao_PromotionDetail> PromotList { get; set; }
        public List<T_ERP_Taobao_ServiceOrder> ServiceOrderList { get; set; }

        #region Model
        private int _id;
        private Guid _guid;
        private decimal? _adjustfee;
        private decimal? _alipayid;
        private string _alipayno;
        private string _alipayurl;
        private string _alipaywarnmsg;
        private string _areaid;
        private decimal? _availableconfirmfee;
        private string _buyeralipayno;
        private string _buyerarea;
        private decimal? _buyercodfee;
        private string _buyeremail;
        private int? _buyerflag;
        private string _buyermemo;
        private string _buyermessage;
        private string _buyernick;
        private decimal? _buyerobtainpointfee;
        private bool? _buyerrate;
        private bool? _canrate;
        private decimal? _codfee;
        private string _codstatus;
        private decimal? _commissionfee;
        private DateTime? _consigntime;
        private DateTime? _created;
        private decimal? _creditcardfee;
        private decimal? _discountfee;
        private DateTime? _endtime;
        private string _eticketext;
        private decimal? _expressagencyfee;
        private bool? _hasbuyermessage;
        private bool? _haspostfee;
        private bool? _hasyfx;
        private string _iid;
        private string _invoicename;
        private bool? _is3d;
        private bool? _isbrandsale;
        private bool? _isforcewlb;
        private bool? _islgtype;
        private DateTime? _lgaging;
        private string _lgagingtype;
        private string _markdesc;
        private DateTime? _modified;
        private decimal? _num;
        private decimal? _numiid;
        private string _nutfeature;
        private Guid _orders;
        private decimal? _payment;
        private DateTime? _paytime;
        private string _picpath;
        private decimal? _pointfee;
        private decimal? _postfee;
        private decimal? _price;
        private string _promotion;
        private Guid _promotiondetails;
        private decimal? _realpointfee;
        private decimal? _receivedpayment;
        private string _receiveraddress;
        private string _receivercity;
        private string _receiverdistrict;
        private string _receivermobile;
        private string _receivername;
        private string _receiverphone;
        private string _receiverstate;
        private string _receiverzip;
        private string _selleralipayno;
        private decimal? _sellercodfee;
        private string _selleremail;
        private int? _sellerflag;
        private string _sellermemo;
        private string _sellermobile;
        private string _sellername;
        private string _sellernick;
        private string _sellerphone;
        private bool? _sellerrate;
        private Guid _serviceorders;
        private string _shippingtype;
        private string _snapshot;
        private string _snapshoturl;
        private string _status;
        private decimal? _steppaidfee;
        private string _steptradestatus;
        private decimal? _tid;
        private DateTime? _timeoutactiontime;
        private string _title;
        private decimal? _totalfee;
        private string _tradefrom;
        private string _tradememo;
        private string _type;
        private decimal? _yfxfee;
        private string _yfxid;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? AdjustFee
        {
            set { _adjustfee = value; }
            get { return _adjustfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? AlipayId
        {
            set { _alipayid = value; }
            get { return _alipayid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlipayNo
        {
            set { _alipayno = value; }
            get { return _alipayno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlipayUrl
        {
            set { _alipayurl = value; }
            get { return _alipayurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AlipayWarnMsg
        {
            set { _alipaywarnmsg = value; }
            get { return _alipaywarnmsg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AreaId
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? AvailableConfirmFee
        {
            set { _availableconfirmfee = value; }
            get { return _availableconfirmfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerAlipayNo
        {
            set { _buyeralipayno = value; }
            get { return _buyeralipayno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerArea
        {
            set { _buyerarea = value; }
            get { return _buyerarea; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? BuyerCodFee
        {
            set { _buyercodfee = value; }
            get { return _buyercodfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerEmail
        {
            set { _buyeremail = value; }
            get { return _buyeremail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BuyerFlag
        {
            set { _buyerflag = value; }
            get { return _buyerflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerMemo
        {
            set { _buyermemo = value; }
            get { return _buyermemo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerMessage
        {
            set { _buyermessage = value; }
            get { return _buyermessage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerNick
        {
            set { _buyernick = value; }
            get { return _buyernick; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? BuyerObtainPointFee
        {
            set { _buyerobtainpointfee = value; }
            get { return _buyerobtainpointfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? BuyerRate
        {
            set { _buyerrate = value; }
            get { return _buyerrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? CanRate
        {
            set { _canrate = value; }
            get { return _canrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? CodFee
        {
            set { _codfee = value; }
            get { return _codfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CodStatus
        {
            set { _codstatus = value; }
            get { return _codstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? CommissionFee
        {
            set { _commissionfee = value; }
            get { return _commissionfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ConsignTime
        {
            set { _consigntime = value; }
            get { return _consigntime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? CreditCardFee
        {
            set { _creditcardfee = value; }
            get { return _creditcardfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? DiscountFee
        {
            set { _discountfee = value; }
            get { return _discountfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EticketExt
        {
            set { _eticketext = value; }
            get { return _eticketext; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ExpressAgencyFee
        {
            set { _expressagencyfee = value; }
            get { return _expressagencyfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? HasBuyerMessage
        {
            set { _hasbuyermessage = value; }
            get { return _hasbuyermessage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? HasPostFee
        {
            set { _haspostfee = value; }
            get { return _haspostfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? HasYfx
        {
            set { _hasyfx = value; }
            get { return _hasyfx; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Iid
        {
            set { _iid = value; }
            get { return _iid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InvoiceName
        {
            set { _invoicename = value; }
            get { return _invoicename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? Is3D
        {
            set { _is3d = value; }
            get { return _is3d; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsBrandSale
        {
            set { _isbrandsale = value; }
            get { return _isbrandsale; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsForceWlb
        {
            set { _isforcewlb = value; }
            get { return _isforcewlb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsLgtype
        {
            set { _islgtype = value; }
            get { return _islgtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LgAging
        {
            set { _lgaging = value; }
            get { return _lgaging; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LgAgingType
        {
            set { _lgagingtype = value; }
            get { return _lgagingtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MarkDesc
        {
            set { _markdesc = value; }
            get { return _markdesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified
        {
            set { _modified = value; }
            get { return _modified; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? NumIid
        {
            set { _numiid = value; }
            get { return _numiid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NutFeature
        {
            set { _nutfeature = value; }
            get { return _nutfeature; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Orders
        {
            set { _orders = value; }
            get { return _orders; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Payment
        {
            set { _payment = value; }
            get { return _payment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? PayTime
        {
            set { _paytime = value; }
            get { return _paytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PicPath
        {
            set { _picpath = value; }
            get { return _picpath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PointFee
        {
            set { _pointfee = value; }
            get { return _pointfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? PostFee
        {
            set { _postfee = value; }
            get { return _postfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Promotion
        {
            set { _promotion = value; }
            get { return _promotion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid PromotionDetails
        {
            set { _promotiondetails = value; }
            get { return _promotiondetails; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? RealPointFee
        {
            set { _realpointfee = value; }
            get { return _realpointfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ReceivedPayment
        {
            set { _receivedpayment = value; }
            get { return _receivedpayment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverAddress
        {
            set { _receiveraddress = value; }
            get { return _receiveraddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverCity
        {
            set { _receivercity = value; }
            get { return _receivercity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverDistrict
        {
            set { _receiverdistrict = value; }
            get { return _receiverdistrict; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverMobile
        {
            set { _receivermobile = value; }
            get { return _receivermobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverName
        {
            set { _receivername = value; }
            get { return _receivername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverPhone
        {
            set { _receiverphone = value; }
            get { return _receiverphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverState
        {
            set { _receiverstate = value; }
            get { return _receiverstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverZip
        {
            set { _receiverzip = value; }
            get { return _receiverzip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerAlipayNo
        {
            set { _selleralipayno = value; }
            get { return _selleralipayno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SellerCodFee
        {
            set { _sellercodfee = value; }
            get { return _sellercodfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerEmail
        {
            set { _selleremail = value; }
            get { return _selleremail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SellerFlag
        {
            set { _sellerflag = value; }
            get { return _sellerflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerMemo
        {
            set { _sellermemo = value; }
            get { return _sellermemo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerMobile
        {
            set { _sellermobile = value; }
            get { return _sellermobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerName
        {
            set { _sellername = value; }
            get { return _sellername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerNick
        {
            set { _sellernick = value; }
            get { return _sellernick; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerPhone
        {
            set { _sellerphone = value; }
            get { return _sellerphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? SellerRate
        {
            set { _sellerrate = value; }
            get { return _sellerrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid ServiceOrders
        {
            set { _serviceorders = value; }
            get { return _serviceorders; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShippingType
        {
            set { _shippingtype = value; }
            get { return _shippingtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Snapshot
        {
            set { _snapshot = value; }
            get { return _snapshot; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SnapshotUrl
        {
            set { _snapshoturl = value; }
            get { return _snapshoturl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? StepPaidFee
        {
            set { _steppaidfee = value; }
            get { return _steppaidfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StepTradeStatus
        {
            set { _steptradestatus = value; }
            get { return _steptradestatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Tid
        {
            set { _tid = value; }
            get { return _tid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? TimeoutActionTime
        {
            set { _timeoutactiontime = value; }
            get { return _timeoutactiontime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? TotalFee
        {
            set { _totalfee = value; }
            get { return _totalfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TradeFrom
        {
            set { _tradefrom = value; }
            get { return _tradefrom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TradeMemo
        {
            set { _tradememo = value; }
            get { return _tradememo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YfxFee
        {
            set { _yfxfee = value; }
            get { return _yfxfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YfxId
        {
            set { _yfxid = value; }
            get { return _yfxid; }
        }
        #endregion Model

    }

    /// <summary>
    /// T_ERP_TaoBao_Order:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_ERP_TaoBao_Order
    {
        public T_ERP_TaoBao_Order()
        { }
        public long Tid { get; set; }
        #region Model
        private int _id;
        private Guid _guid;
        private decimal? _adjustfee;
        private string _buyernick;
        private bool? _buyerrate;
        private decimal? _cid;
        private decimal? _discountfee;
        private DateTime? _endtime;
        private string _iid;
        private bool? _isoversold;
        private bool? _isserviceorder;
        private decimal? _itemmealid;
        private string _itemmealname;
        private DateTime? _modified;
        private decimal? _num;
        private decimal? _numiid;
        private decimal? _oid;
        private string _orderfrom;
        private string _outeriid;
        private string _outerskuid;
        private decimal? _payment;
        private string _picpath;
        private decimal? _price;
        private decimal? _refundid;
        private string _refundstatus;
        private string _sellernick;
        private bool? _sellerrate;
        private string _sellertype;
        private string _skuid;
        private string _skupropertiesname;
        private string _snapshot;
        private string _snapshoturl;
        private string _status;
        private DateTime? _timeoutactiontime;
        private string _title;
        private decimal? _totalfee;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? AdjustFee
        {
            set { _adjustfee = value; }
            get { return _adjustfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyerNick
        {
            set { _buyernick = value; }
            get { return _buyernick; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? BuyerRate
        {
            set { _buyerrate = value; }
            get { return _buyerrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Cid
        {
            set { _cid = value; }
            get { return _cid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? DiscountFee
        {
            set { _discountfee = value; }
            get { return _discountfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Iid
        {
            set { _iid = value; }
            get { return _iid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsOversold
        {
            set { _isoversold = value; }
            get { return _isoversold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsServiceOrder
        {
            set { _isserviceorder = value; }
            get { return _isserviceorder; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ItemMealId
        {
            set { _itemmealid = value; }
            get { return _itemmealid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemMealName
        {
            set { _itemmealname = value; }
            get { return _itemmealname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified
        {
            set { _modified = value; }
            get { return _modified; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? NumIid
        {
            set { _numiid = value; }
            get { return _numiid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Oid
        {
            set { _oid = value; }
            get { return _oid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderFrom
        {
            set { _orderfrom = value; }
            get { return _orderfrom; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OuterIid
        {
            set { _outeriid = value; }
            get { return _outeriid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OuterSkuId
        {
            set { _outerskuid = value; }
            get { return _outerskuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Payment
        {
            set { _payment = value; }
            get { return _payment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PicPath
        {
            set { _picpath = value; }
            get { return _picpath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? RefundId
        {
            set { _refundid = value; }
            get { return _refundid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefundStatus
        {
            set { _refundstatus = value; }
            get { return _refundstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerNick
        {
            set { _sellernick = value; }
            get { return _sellernick; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? SellerRate
        {
            set { _sellerrate = value; }
            get { return _sellerrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SellerType
        {
            set { _sellertype = value; }
            get { return _sellertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SkuId
        {
            set { _skuid = value; }
            get { return _skuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SkuPropertiesName
        {
            set { _skupropertiesname = value; }
            get { return _skupropertiesname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Snapshot
        {
            set { _snapshot = value; }
            get { return _snapshot; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SnapshotUrl
        {
            set { _snapshoturl = value; }
            get { return _snapshoturl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? TimeoutActionTime
        {
            set { _timeoutactiontime = value; }
            get { return _timeoutactiontime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? TotalFee
        {
            set { _totalfee = value; }
            get { return _totalfee; }
        }
        #endregion Model

    }

    /// <summary>
    /// T_ERP_TaoBao_PromotionDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_ERP_TaoBao_PromotionDetail
    {
        public T_ERP_TaoBao_PromotionDetail()
        { }
        public long Tid { get; set; }
        #region Model
        private int _pid;
        private Guid _guid;
        private decimal? _discountfee;
        private string _giftitemid;
        private string _giftitemname;
        private decimal? _giftitemnum;
        private decimal? _id;
        private string _promotiondesc;
        private string _promotionid;
        private string _promotionname;
        private bool? _isusing;
        /// <summary>
        /// 
        /// </summary>
        public int PId
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? DiscountFee
        {
            set { _discountfee = value; }
            get { return _discountfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GiftItemId
        {
            set { _giftitemid = value; }
            get { return _giftitemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GiftItemName
        {
            set { _giftitemname = value; }
            get { return _giftitemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? GiftItemNum
        {
            set { _giftitemnum = value; }
            get { return _giftitemnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PromotionDesc
        {
            set { _promotiondesc = value; }
            get { return _promotiondesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PromotionId
        {
            set { _promotionid = value; }
            get { return _promotionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PromotionName
        {
            set { _promotionname = value; }
            get { return _promotionname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsUsing
        {
            set { _isusing = value; }
            get { return _isusing; }
        }
        #endregion Model

    }

    [Serializable]
    public class T_ERP_Taobao_ServiceOrder
    {
        public int Id { get; set; }

        public long Tid { get; set; }

        public Guid Guid { get; set; }

        /// <summary>
        /// 卖家昵称
        /// </summary>
        [XmlElement("buyer_nick")]
        public string BuyerNick { get; set; }

        /// <summary>
        /// 服务所属的交易订单号。如果服务为一年包换，则item_oid这笔订单享受改服务的保护
        /// </summary>
        [XmlElement("item_oid")]
        public long ItemOid { get; set; }

        /// <summary>
        /// 购买数量，取值范围为大于0的整数
        /// </summary>
        [XmlElement("num")]
        public long Num { get; set; }

        /// <summary>
        /// 虚拟服务子订单订单号
        /// </summary>
        [XmlElement("oid")]
        public long Oid { get; set; }

        /// <summary>
        /// 子订单实付金额。精确到2位小数，单位:元。如:200.07，表示:200元7分。
        /// </summary>
        [XmlElement("payment")]
        public decimal? Payment { get; set; }

        /// <summary>
        /// 服务图片地址
        /// </summary>
        [XmlElement("pic_path")]
        public string PicPath { get; set; }

        /// <summary>
        /// 服务价格，精确到小数点后两位：单位:元
        /// </summary>
        [XmlElement("price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 最近退款的id
        /// </summary>
        [XmlElement("refund_id")]
        public long RefundId { get; set; }

        /// <summary>
        /// 卖家昵称
        /// </summary>
        [XmlElement("seller_nick")]
        public string SellerNick { get; set; }

        /// <summary>
        /// 服务详情的URL地址
        /// </summary>
        [XmlElement("service_detail_url")]
        public string ServiceDetailUrl { get; set; }

        /// <summary>
        /// 服务数字id
        /// </summary>
        [XmlElement("service_id")]
        public long ServiceId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 服务子订单总费用
        /// </summary>
        [XmlElement("total_fee")]
        public decimal? TotalFee { get; set; }
    }
    public static class ExMethod
    {
        public static DateTime? ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            DateTime de;
            if (!DateTime.TryParse(str, out de)) return null;
            return de;
        }

        public static bool IsEmpty(this string str) { return string.IsNullOrEmpty(str); }
    }

    [Serializable]
    public class T_ERP_TaoBao_Refund
    {
        public string Address { get; set; }
        public long AdvanceStatus { get; set; }
        public string AlipayNo { get; set; }
        public string BuyerNick { get; set; }
        public string CompanyName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? CreateTime { get; set; }
        public long CsStatus { get; set; }
        public bool ExistTimeout { get; set; }
        public DateTime? GoodReturnTime { get; set; }
        public string GoodStatus { get; set; }
        public Guid? Guid { get; set; }
        public bool HasGoodReturn { get; set; }
        public int Id { get; set; }
        public string Iid { get; set; }
        public bool IsMoved { get; set; }
        public DateTime? Modified { get; set; }
        public long Num { get; set; }
        public long NumIid { get; set; }
        public long Oid { get; set; }
        public string OrderStatus { get; set; }
        public string Payment { get; set; }
        public string Price { get; set; }
        public string Reason { get; set; }
        public string RefundFee { get; set; }
        public long RefundId { get; set; }
        public long RemindType { get; set; }
        public string SellerNick { get; set; }
        public string ShippingType { get; set; }
        public string Sid { get; set; }
        public string SplitSellerFee { get; set; }
        public string SplitTaobaoFee { get; set; }
        public string Status { get; set; }
        public long Tid { get; set; }
        public DateTime? Timeout { get; set; }
        public string Title { get; set; }
        public string TotalFee { get; set; }
    }

    [Serializable]
    public class T_ERP_ShopRefund
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public string SellerNick { get; set; }
        public int JdpCount { get; set; }
    }

    [Serializable]
    public class T_ERP_ShopTrade
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public string SellerNick { get; set; }
        public int JdpCount { get; set; }
        public int TrdStep { get; set; }
        public int TrdPayed { get; set; }
        public int TrdUnPay { get; set; }
    }

    [Serializable]
    public class T_ERP_WriteFileErr
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreateTime { get; set; }
        public string FailFileName { get; set; }
        public string TypeName { get; set; }
        public string JsonText { get; set; }
    }

    public class Caster
    {
        public static List<T_ERP_TaoBao_Trade> CastTrades(List<Top.Api.Domain.Trade> trds)
        {
            List<T_ERP_TaoBao_Trade> lstbrd = new List<T_ERP_TaoBao_Trade>(trds.Count);
            trds.ForEach(ki => { lstbrd.Add(Caster.Cast(ki)); });
            return lstbrd;
        }

        public static T_ERP_TaoBao_Trade Cast(Top.Api.Domain.Trade trd)
        {
            if (null == trd) return null;
            Guid gidTrade = Guid.NewGuid();
            return new T_ERP_TaoBao_Trade
            {
                AdjustFee = trd.AdjustFee.ToDecimal(),
                AlipayId = trd.AlipayId,
                AlipayNo = trd.AlipayNo,
                AlipayUrl = trd.AlipayUrl,
                AlipayWarnMsg = trd.AlipayWarnMsg,
                AreaId = trd.AreaId,
                AvailableConfirmFee = trd.AvailableConfirmFee.ToDecimal(),
                BuyerAlipayNo = trd.BuyerAlipayNo,
                BuyerArea = trd.BuyerArea,
                BuyerCodFee = trd.BuyerCodFee.ToDecimal(),
                BuyerEmail = trd.BuyerEmail,
                BuyerFlag = trd.BuyerFlag.ToInt(),
                BuyerMemo = trd.BuyerMemo,
                BuyerMessage = trd.BuyerMessage,
                BuyerNick = trd.BuyerNick,
                BuyerObtainPointFee = trd.BuyerObtainPointFee,
                BuyerRate = trd.BuyerRate,
                CanRate = trd.CanRate,
                CodFee = trd.CodFee.ToDecimal(),
                CodStatus = trd.CodStatus,
                CommissionFee = trd.CommissionFee.ToDecimal(),
                ConsignTime = trd.ConsignTime.ToDateTime(),
                Created = trd.Created.ToDateTime(),
                CreditCardFee = trd.CreditCardFee.ToDecimal(),
                DiscountFee = trd.DiscountFee.ToDecimal(),
                EndTime = trd.EndTime.ToDateTime(),
                EticketExt = trd.EticketExt,
                ExpressAgencyFee = trd.ExpressAgencyFee.ToDecimal(),
                HasBuyerMessage = trd.HasBuyerMessage,
                HasPostFee = trd.HasPostFee,
                HasYfx = trd.HasYfx,
                Iid = trd.Iid,
                InvoiceName = trd.InvoiceName,
                Is3D = trd.Is3D,
                IsBrandSale = trd.IsBrandSale,
                IsForceWlb = trd.IsForceWlb,
                IsLgtype = trd.IsLgtype,
                LgAging = trd.LgAging.ToDateTime(),
                LgAgingType = trd.LgAgingType,
                MarkDesc = trd.MarkDesc,
                Modified = trd.Modified.ToDateTime(),
                Num = trd.Num,
                NumIid = trd.NumIid,
                NutFeature = trd.NutFeature,
                Payment = trd.Payment.ToDecimal(),
                PayTime = trd.PayTime.ToDateTime(),
                PicPath = trd.PicPath,
                PointFee = trd.PointFee,
                PostFee = trd.PostFee.ToDecimal(),
                Price = trd.Price.ToDecimal(),
                Promotion = trd.Promotion,
                //PromotionDetails=trd.PromotionDetails,
                //Orders=trd.Orders,
                //ServiceOrders=trd.ServiceOrders
                /**************************************************/
                Guid = gidTrade,
                Orders = gidTrade,
                OrderList = Caster.CastOrders(trd, gidTrade),
                PromotList = Caster.CastPromot(trd, gidTrade),
                PromotionDetails = gidTrade,
                ServiceOrderList = Caster.CastServiceOrder(trd, gidTrade),
                ServiceOrders = gidTrade,
                /**************************************************/
                RealPointFee = trd.RealPointFee,
                ReceivedPayment = trd.ReceivedPayment.ToDecimal(),
                ReceiverAddress = trd.ReceiverAddress,
                ReceiverCity = trd.ReceiverCity,
                ReceiverDistrict = trd.ReceiverDistrict,
                ReceiverMobile = trd.ReceiverMobile,
                ReceiverName = trd.ReceiverName,
                ReceiverPhone = trd.ReceiverPhone,
                ReceiverState = trd.ReceiverState,
                ReceiverZip = trd.ReceiverZip,
                SellerAlipayNo = trd.SellerAlipayNo,
                SellerCodFee = trd.SellerCodFee.ToDecimal(),
                SellerEmail = trd.SellerEmail,
                SellerFlag = trd.SellerFlag.ToInt(),
                SellerMemo = trd.SellerMemo,
                SellerMobile = trd.SellerMobile,
                SellerName = trd.SellerName,
                SellerNick = trd.SellerNick,
                SellerPhone = trd.SellerPhone,
                SellerRate = trd.SellerRate,
                ShippingType = trd.ShippingType,
                Snapshot = trd.Snapshot,
                SnapshotUrl = trd.SnapshotUrl,
                Status = trd.Status,
                StepPaidFee = trd.StepPaidFee.ToDecimal(),
                StepTradeStatus = trd.StepTradeStatus,
                Tid = trd.Tid,
                TimeoutActionTime = trd.TimeoutActionTime.ToDateTime(),
                Title = trd.Title,
                TotalFee = trd.TotalFee.ToDecimal(),
                TradeFrom = trd.TradeFrom,
                TradeMemo = trd.TradeMemo,
                Type = trd.Type,
                YfxFee = trd.YfxFee.ToDecimal(),
                YfxId = trd.YfxId,
            };

        }

        public static List<T_ERP_TaoBao_Order> CastOrders(Top.Api.Domain.Trade trd, Guid gidTrade)
        {
            if (null == trd || trd.Orders == null) return null;
            List<T_ERP_TaoBao_Order> ret = new List<T_ERP_TaoBao_Order>(trd.Orders.Count);
            trd.Orders.ForEach(k =>
            {
                ret.Add(new T_ERP_TaoBao_Order
                {
                    Tid = trd.Tid,
                    Guid = gidTrade,
                    AdjustFee = k.AdjustFee.ToDecimal(),
                    BuyerNick = k.BuyerNick,
                    BuyerRate = k.BuyerRate,
                    Cid = k.Cid,
                    DiscountFee = k.DiscountFee.ToDecimal(),
                    EndTime = k.EndTime.ToDateTime(),
                    Iid = k.Iid,
                    IsOversold = k.IsOversold,
                    IsServiceOrder = k.IsServiceOrder,
                    ItemMealId = k.ItemMealId,
                    ItemMealName = k.ItemMealName,
                    Modified = k.Modified.ToDateTime(),
                    Num = k.Num,
                    NumIid = k.NumIid,
                    Oid = k.Oid,
                    OrderFrom = k.OrderFrom,
                    OuterIid = k.OuterIid,
                    OuterSkuId = k.OuterSkuId,
                    Payment = k.Payment.ToDecimal(),
                    PicPath = k.PicPath,
                    Price = k.Price.ToDecimal(),
                    RefundId = k.RefundId,
                    RefundStatus = k.RefundStatus,
                    SellerNick = k.SellerNick.IsEmpty() ? trd.SellerNick : k.SellerNick,
                    SellerRate = k.SellerRate,
                    SellerType = k.SellerType,
                    SkuId = k.SkuId,
                    SkuPropertiesName = k.SkuPropertiesName,
                    Snapshot = k.Snapshot,
                    SnapshotUrl = k.SnapshotUrl,
                    Status = k.Status,
                    TimeoutActionTime = k.TimeoutActionTime.ToDateTime(),
                    Title = k.Title,
                    TotalFee = k.TotalFee.ToDecimal()
                });
            });
            return ret;
        }

        public static List<T_ERP_TaoBao_PromotionDetail> CastPromot(Top.Api.Domain.Trade trd, Guid gidTrad)
        {
            if (null == trd || trd.PromotionDetails == null) return null;
            List<T_ERP_TaoBao_PromotionDetail> ret = new List<T_ERP_TaoBao_PromotionDetail>(trd.PromotionDetails.Count);
            trd.PromotionDetails.ForEach(t =>
            {
                ret.Add(new T_ERP_TaoBao_PromotionDetail
                {
                    Tid = trd.Tid,
                    DiscountFee = t.DiscountFee.ToDecimal(),
                    GiftItemId = t.GiftItemId,
                    GiftItemName = t.GiftItemName,
                    GiftItemNum = t.GiftItemNum.ToDecimal(),
                    Guid = gidTrad,
                    PromotionDesc = t.PromotionDesc,
                    PromotionId = t.PromotionId,
                    PromotionName = t.PromotionName,
                });
            });
            return ret;
        }

        public static List<T_ERP_Taobao_ServiceOrder> CastServiceOrder(Top.Api.Domain.Trade trd, Guid gidTrade)
        {
            if (null == trd || trd.ServiceOrders == null) return null;
            var ret = new List<T_ERP_Taobao_ServiceOrder>(trd.ServiceOrders.Count);
            trd.ServiceOrders.ForEach(t =>
            {
                ret.Add(new T_ERP_Taobao_ServiceOrder
                {
                    Tid = trd.Tid,
                    BuyerNick = t.BuyerNick,
                    Guid = gidTrade,
                    ItemOid = t.ItemOid,
                    Num = t.Num,
                    Oid = t.Oid,
                    Payment = t.Payment.ToDecimal(),
                    PicPath = t.PicPath,
                    Price = t.Price.ToDecimal(),
                    RefundId = t.RefundId,
                    SellerNick = t.SellerNick.IsEmpty() ? trd.SellerNick : t.SellerNick,
                    ServiceDetailUrl = t.ServiceDetailUrl,
                    ServiceId = t.ServiceId,
                    Title = t.Title,
                    TotalFee = t.TotalFee.ToDecimal()
                });
            });

            return ret;
        }

        public static List<T_ERP_TaoBao_Refund> CastRefund(List<Top.Api.Domain.Refund> rfd)
        {
            if (null == rfd || rfd.Count < 1) return null;
            return rfd.Select(k =>
            {
                return new T_ERP_TaoBao_Refund
                {
                    Address = k.Address,
                    AdvanceStatus = k.AdvanceStatus,
                    AlipayNo = k.AlipayNo,
                    BuyerNick = k.BuyerNick,
                    CompanyName = k.CompanyName,
                    Created = k.Created.ToDateTime(),
                    CsStatus = k.CsStatus,
                    GoodReturnTime = k.GoodReturnTime.ToDateTime(),
                    GoodStatus = k.GoodStatus,
                    Guid = Guid.NewGuid(),
                    HasGoodReturn = k.HasGoodReturn,
                    Iid = k.Iid,
                    Modified = k.Modified.ToDateTime(),
                    Num = k.Num,
                    NumIid = k.NumIid,
                    Oid = k.Oid,
                    OrderStatus = k.OrderStatus,
                    Payment = k.Payment,
                    Price = k.Price,
                    Reason = k.Reason,
                    RefundFee = k.RefundFee,
                    RefundId = k.RefundId,
                    RemindType = k.RefundRemindTimeout == null ? 1 : k.RefundRemindTimeout.RemindType,
                    SellerNick = k.SellerNick,
                    ShippingType = k.ShippingType,
                    Sid = k.Sid,
                    SplitSellerFee = k.SplitSellerFee,
                    SplitTaobaoFee = k.SplitTaobaoFee,
                    Status = k.Status,
                    Tid = k.Tid,
                    Timeout = k.RefundRemindTimeout == null ? null : k.RefundRemindTimeout.Timeout.ToDateTime(),
                    Title = k.Title,
                    TotalFee = k.TotalFee
                };
            }).ToList();
        }
    }

    /// <summary>
    /// 店铺数据实体
    /// </summary>
    public class ShopData
    {
        public string SellerNick { get; set; }
        public string RefundTableName { get; set; }
        public string TradeTableName { get; set; }
        public int SleepSecAdjust { get; set; }
    }

    public class T_ERP_SyncLog
    {
        public string SellerNick { get; set; }
        public DateTime MinDateTime { get; set; }
        public DateTime MaxDateTime { get; set; }

        private string _Remark = "同步";

        public int JdpCounts { get; set; }

        public int JdpResCounts { get; set; }

        public string Remark { get { return _Remark; } set { _Remark = value; } }
    }

    public class ServerTime
    {
        public DateTime CurrentTime { get; set; }
    }

    /// <summary>
    /// 时间同步类,用于与指定基准时间做同步
    /// </summary>
    public class TimeSync
    {
        private static TimeSync _Default = new TimeSync();

        /// <summary>
        /// 默认实例
        /// </summary>
        public static TimeSync Default { get { return _Default; } }

        /// <summary>
        /// 开始与initTime中指定的时间同步,同步粒度为秒计
        /// </summary>
        /// <param name="initTime">同步开始的基准时间,一般提供服务器的当前时间</param>
        /// <returns>同步后的时间</returns>
        public DateTime StartSync(DateTime initTime)
        {
            StopSync();
            CurrentSyncTime = initTime;
            tSync = new System.Threading.Thread(() => { SyncProc(); });
            tSync.IsBackground = true;
            tSync.Start();
            return CurrentSyncTime;
        }

        /// <summary>
        /// 停止时间同步
        /// </summary>
        /// <returns></returns>
        public TimeSync StopSync()
        {
            try
            {
                if (null != tSync && tSync.IsAlive) { tSync.Abort(); }
            }
#if DEBUG
            catch (Exception) { throw; }
#else
#endif
            finally { tSync = null; }
            return this;
        }

        /// <summary>
        /// 当前时间,NOTE:非本地时间
        /// </summary>
        public virtual DateTime CurrentSyncTime { get; protected set; }

        /// <summary>
        /// 时间同步过程
        /// </summary>
        protected virtual void SyncProc()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                CurrentSyncTime = CurrentSyncTime.AddSeconds(1);
            }
        }

        /// <summary>
        /// 时间同步线程
        /// </summary>
        protected System.Threading.Thread tSync { get; set; }
    }

    [Serializable]
    public class Jdp {
        public long tid { get; set; }
        public string seller_nick { get; set; }
        public string jdp_response { get; set; }
    }

    [Serializable]
    public class RefJdp {
        public long refund_id { get; set; }
        public long tid { get; set; }
        public string seller_nick { get; set; }
        public string jdp_response { get; set; }
    }
}
