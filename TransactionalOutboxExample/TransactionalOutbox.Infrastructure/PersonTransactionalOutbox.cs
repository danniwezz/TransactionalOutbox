using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.TransactionalOutbox;
using Shared.TransactionalOutbox.MessageRelayService;
using Shared.TransactionalOutbox.Models;
using System.Text.Json;

namespace TransactionalOutbox.Infrastructure;
public class PersonTransactionalOutbox : TransactionalOutbox<PersonTransactionalOutbox>
{
	public PersonTransactionalOutbox(TransactionalOutboxEfCoreConfiguration configuration, IMessageRelayServiceNotifier messageRelayServiceNotifier, ILogger<TransactionalOutbox<PersonTransactionalOutbox>> logger, JsonSerializerOptions serializerOptions) : base(configuration, messageRelayServiceNotifier, logger, serializerOptions) { }
}
