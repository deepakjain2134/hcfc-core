using HCF.Module.Core.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HCF.Module.News.Events
{
    public class UserSignedInHandler : INotificationHandler<UserSignedIn>
    {

        public UserSignedInHandler()
        {

        }

        public async Task Handle(UserSignedIn user, CancellationToken cancellationToken)
        {
        }
    }
}
