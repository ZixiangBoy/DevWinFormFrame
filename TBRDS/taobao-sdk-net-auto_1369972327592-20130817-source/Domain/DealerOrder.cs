using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// DealerOrder Data Structure.
    /// </summary>
    [Serializable]
    public class DealerOrder : TopObject
    {
        /// <summary>
        /// 申请时间
        /// </summary>
        [XmlElement("applied_time")]
        public string AppliedTime { get; set; }

        /// <summary>
        /// 采购商nick
        /// </summary>
        [XmlElement("applier_nick")]
        public string ApplierNick { get; set; }

        /// <summary>
        /// 采购商审核通过时间
        /// </summary>
        [XmlElement("audit_time_applier")]
        public string AuditTimeApplier { get; set; }

        /// <summary>
        /// 供货商审核通过时间
        /// </summary>
        [XmlElement("audit_time_supplier")]
        public string AuditTimeSupplier { get; set; }

        /// <summary>
        /// 关闭原因
        /// </summary>
        [XmlElement("close_reason")]
        public string CloseReason { get; set; }

        /// <summary>
        /// 产品明细
        /// </summary>
        [XmlArray("dealer_order_details")]
        [XmlArrayItem("dealer_order_detail")]
        public List<DealerOrderDetail> DealerOrderDetails { get; set; }

        /// <summary>
        /// 采购申请单编号
        /// </summary>
        [XmlElement("dealer_order_id")]
        public long DealerOrderId { get; set; }

        /// <summary>
        /// 出库数
        /// </summary>
        [XmlElement("delivered_quantity_count")]
        public long DeliveredQuantityCount { get; set; }

        /// <summary>
        /// 物流费用(精确到2位小数;单位:元。如:200.07，表示:200元7分 )
        /// </summary>
        [XmlElement("logistics_fee")]
        public string LogisticsFee { get; set; }

        /// <summary>
        /// 物流方式：  SELF_PICKUP（自提）、LOGISTICS（物流发货)
        /// </summary>
        [XmlElement("logistics_type")]
        public string LogisticsType { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [XmlElement("modified_time")]
        public string ModifiedTime { get; set; }

        /// <summary>
        /// WAIT_FOR_SUPPLIER_AUDIT1：采购方刚申请，待供货方审核；  SUPPLIER_REFUSE：供货方驳回，待采购方审核；  WAIT_FOR_APPLIER_AUDIT：供货方修改后，待采购方审核；  WAIT_FOR_SUPPLIER_AUDIT2：采购方驳回修改，待供货方再审核；  BOTH_AGREE_WAIT_PAY：双方审核通过，待采购方付款；  WAIT_CONFIRM_PAID：采购方已付款，待供货方确认；  WAIT_FOR_SUPPLIER_DELIVER：付款成功，待供货方出库；  WAIT_FOR_APPLIER_STORAGE：供货方出库，待采购方入库；  TRADE_FINISHED：采购方入库，交易成功；  TRADE_CLOSED：采购申请单关闭。
        /// </summary>
        [XmlElement("order_status")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// 支付方式：  ALIPAY_SURETY（支付宝担保交易）  TRANSFER（线下转账）  PREPAY（预存款）  IMMEDIATELY（即时到账）
        /// </summary>
        [XmlElement("pay_type")]
        public string PayType { get; set; }

        /// <summary>
        /// 总采购数量
        /// </summary>
        [XmlElement("quantity_count")]
        public long QuantityCount { get; set; }

        /// <summary>
        /// 收货人信息
        /// </summary>
        [XmlElement("receiver")]
        public Receiver Receiver { get; set; }

        /// <summary>
        /// 采购商驳回原因
        /// </summary>
        [XmlElement("refuse_reason_applier")]
        public string RefuseReasonApplier { get; set; }

        /// <summary>
        /// 供货商驳回原因
        /// </summary>
        [XmlElement("refuse_reason_supplier")]
        public string RefuseReasonSupplier { get; set; }

        /// <summary>
        /// 供货商nick
        /// </summary>
        [XmlElement("supplier_nick")]
        public string SupplierNick { get; set; }

        /// <summary>
        /// 采购总价(精确到2位小数;单位:元。如:200.07，表示:200元7分 )
        /// </summary>
        [XmlElement("total_price")]
        public string TotalPrice { get; set; }
    }
}
