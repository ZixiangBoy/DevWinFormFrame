using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// FenxiaoDealerRequisitionorderQueryResponse.
    /// </summary>
    public class FenxiaoDealerRequisitionorderQueryResponse : TopResponse
    {
        /// <summary>
        /// 采购申请单结果列表
        /// </summary>
        [XmlArray("dealer_orders")]
        [XmlArrayItem("dealer_order")]
        public List<DealerOrder> DealerOrders { get; set; }
    }
}
