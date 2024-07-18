using Cassandra.Mapping;

namespace CassandraConnectSample.Mapping;

public class PaymentMapping : Mappings
{
    public PaymentMapping()
    {
        For<Payment>()
            .TableName("payments")
            .PartitionKey(p => p.Id)
            .Column(p => p.OrderId, cm => cm.WithName("orderid"))
            .Column(p => p.Amount, cm => cm.WithName("amount"))
            .Column(p => p.PaymentDate, cm => cm.WithName("paymentdate"))
            .Column(p => p.Crtd_Usr, cm => cm.WithName("crtd_usr"))
            .Column(p => p.Crtd_Dt, cm => cm.WithName("crtd_dt"))
            .Column(p => p.Lst_Crtd_Usr, cm => cm.WithName("lst_crtd_usr"))
            .Column(p => p.Lst_Crtd_Dt, cm => cm.WithName("lst_crtd_dt"))
            .Column(p => p.Actv_Ind, cm => cm.WithName("actv_ind"))
            .Column(p => p.Del_Ind, cm => cm.WithName("del_ind"));
    }
}
