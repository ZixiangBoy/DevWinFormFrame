using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.fenxiao.dealer.requisitionorder.get
    /// </summary>
    public class FenxiaoDealerRequisitionorderGetRequest : ITopRequest<FenxiaoDealerRequisitionorderGetResponse>
    {
        /// <summary>
        /// 采购申请单最迟修改时间。与start_date字段的最大时间间隔不能超过30天
        /// </summary>
        public Nullable<DateTime> EndDate { get; set; }

        /// <summary>
        /// 多个字段用","分隔。 fields 如果为空：返回所有采购申请单对象(dealer_orders)字段。 如果不为空：返回指定采购单对象(dealer_orders)字段。 例1： dealer_order_details.product_id 表示只返回product_id 例2： dealer_order_details 表示只返回明细列表
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 查询者自己在所要查询的采购申请单中的身份。  1：供货方。  2：采购方。  注：填写其他值当做错误处理。
        /// </summary>
        public Nullable<long> Identity { get; set; }

        /// <summary>
        /// 采购申请单状态。  0：全部状态。  1：采购方刚申请，待供货方审核。  2：供货方驳回，待采购方审核。  3：供货方修改后，待采购方审核。  4：采购方驳回修改，待供货方再审核。  5：双方审核通过，待采购方付款。  6：采购方已付款，待供货方确认。  7：付款成功，待供货方出库。  8：供货方出库，待采购方入库。  9：采购方入库，交易成功。  10：采购申请单关闭。    注：无值按默认值0计，超出状态范围返回错误信息。
        /// </summary>
        public Nullable<long> OrderStatus { get; set; }

        /// <summary>
        /// 页码（大于0的整数。无值或小于1的值按默认值1计）
        /// </summary>
        public Nullable<long> PageNo { get; set; }

        /// <summary>
        /// 每页条数（大于0但小于等于50的整数。无值或大于50或小于1的值按默认值50计）
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 采购申请单最早修改时间
        /// </summary>
        public Nullable<DateTime> StartDate { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.fenxiao.dealer.requisitionorder.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("end_date", this.EndDate);
            parameters.Add("fields", this.Fields);
            parameters.Add("identity", this.Identity);
            parameters.Add("order_status", this.OrderStatus);
            parameters.Add("page_no", this.PageNo);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("start_date", this.StartDate);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("end_date", this.EndDate);
            RequestValidator.ValidateRequired("identity", this.Identity);
            RequestValidator.ValidateRequired("start_date", this.StartDate);
        }

        #endregion

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new TopDictionary();
            }
            this.otherParameters.Add(key, value);
        }
    }
}
