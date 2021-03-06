﻿using Backend.Library.Payment.Enums;
using MediatR;

namespace Backend.Business.Billing.BillingRequests.GetSubscriptionStatus
{
    public class GetSubscriptionStatusRequest : IRequest<SubscriptionStatus>
    {
        public string Id { get; set; }

        public GetSubscriptionStatusRequest(string id)
        {
            Id = id;
        }
    }
}