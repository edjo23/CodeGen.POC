/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable

using Beef;
using Beef.Business;
using Beef.Entities;
using Beef.Validation;
using Demo.Service.Business.DataSvc;
using Demo.Service.Business.Validation;
using Demo.Service.Common.Entities;
using System;
using System.Threading.Tasks;

namespace Demo.Service.Business
{
    public partial class PersonManager : IPersonManager
    {
        public PersonManager(IPersonDataSvc dataService)
        {
            _dataService = dataService;
        }

        #region Private

        private readonly IPersonDataSvc _dataService;

        private readonly Func<Person, Task>? _OnPreValidateCreateAsync;
        private readonly Action<MultiValidator, Person>? _OnCreateValidate;
        private readonly Func<Person, Task>? _OnBeforeCreateAsync;
        private readonly Func<Person, Task>? _OnAfterCreateAsync;

        private readonly Func<Guid, Task>? _OnPreValidateGetAsync;
        private readonly Action<MultiValidator, Guid>? _OnGetValidate;
        private readonly Func<Guid, Task>? _OnBeforeGetAsync;
        private readonly Func<Person?, Guid, Task>? _OnAfterGetAsync;

        private readonly Func<Person, Task>? _OnPreValidateUpdateAsync;
        private readonly Action<MultiValidator, Person>? _OnUpdateValidate;
        private readonly Func<Person, Task>? _OnBeforeUpdateAsync;
        private readonly Func<Person, Task>? _OnAfterUpdateAsync;

        private readonly Func<Guid, Task>? _OnPreValidateDeleteAsync;
        private readonly Action<MultiValidator, Guid>? _OnDeleteValidate;
        private readonly Func<Guid, Task>? _OnBeforeDeleteAsync;
        private readonly Func<Guid, Task>? _OnAfterDeleteAsync;

        private readonly Func<PersonArgs, PagingArgs?, Task>? _OnPreValidateGetByArgsAsync;
        private readonly Action<MultiValidator, PersonArgs, PagingArgs?>? _OnGetByArgsValidate;
        private readonly Func<PersonArgs, PagingArgs?, Task>? _OnBeforeGetByArgsAsync;
        private readonly Func<PersonCollectionResult, PersonArgs, PagingArgs?, Task>? _OnAfterGetByArgsAsync;

        #endregion

        public Task<Person> CreateAsync(Person value)
        {
            return ManagerInvoker.Default.InvokeAsync(this, async () =>
            {
                ExecutionContext.Current.OperationType = OperationType.Create;                
                EntityBase.CleanUp(value);
                if (_OnPreValidateCreateAsync != null) await _OnPreValidateCreateAsync(value).ConfigureAwait(false);

                MultiValidator.Create()
                    .Add(value.Validate(nameof(value)).Mandatory())
                    .Add(value.Validate(nameof(value)).Entity(PersonValidator.Default))
                    .Additional((__mv) => _OnCreateValidate?.Invoke(__mv, value))
                    .Run().ThrowOnError();

                if (_OnBeforeCreateAsync != null) await _OnBeforeCreateAsync(value).ConfigureAwait(false);
                var __result = await _dataService.CreateAsync(value).ConfigureAwait(false);
                if (_OnAfterCreateAsync != null) await _OnAfterCreateAsync(__result).ConfigureAwait(false);
                Cleaner.Clean(__result);
                return __result;
            });
        }

        public Task<Person?> GetAsync(Guid id)
        {
            return ManagerInvoker.Default.InvokeAsync(this, async () =>
            {
                ExecutionContext.Current.OperationType = OperationType.Read;
                EntityBase.CleanUp(id);
                if (_OnPreValidateGetAsync != null) await _OnPreValidateGetAsync(id).ConfigureAwait(false);

                MultiValidator.Create()
                    .Add(id.Validate(nameof(id)).Mandatory())
                    .Additional((__mv) => _OnGetValidate?.Invoke(__mv, id))
                    .Run().ThrowOnError();

                if (_OnBeforeGetAsync != null) await _OnBeforeGetAsync(id).ConfigureAwait(false);
                var __result = await _dataService.GetAsync(id).ConfigureAwait(false);
                if (_OnAfterGetAsync != null) await _OnAfterGetAsync(__result, id).ConfigureAwait(false);
                Cleaner.Clean(__result);
                return __result;
            });
        }

        public Task<Person> UpdateAsync(Person value)
        {
            return ManagerInvoker.Default.InvokeAsync(this, async () =>
            {
                ExecutionContext.Current.OperationType = OperationType.Update;                
                EntityBase.CleanUp(value);
                if (_OnPreValidateUpdateAsync != null) await _OnPreValidateUpdateAsync(value).ConfigureAwait(false);

                MultiValidator.Create()                    
                    .Add(value.Validate(nameof(value)).Mandatory())
                    .Add(value.Validate(nameof(value)).Entity(PersonValidator.Default))
                    .Additional((__mv) => _OnUpdateValidate?.Invoke(__mv, value))
                    .Run().ThrowOnError();

                if (_OnBeforeUpdateAsync != null) await _OnBeforeUpdateAsync(value).ConfigureAwait(false);
                var __result = await _dataService.UpdateAsync(value).ConfigureAwait(false);
                if (_OnAfterUpdateAsync != null) await _OnAfterUpdateAsync(__result).ConfigureAwait(false);
                Cleaner.Clean(__result);
                return __result;
            });
        }

        public Task DeleteAsync(Guid id)
        {
            return ManagerInvoker.Default.InvokeAsync(this, async () =>
            {
                ExecutionContext.Current.OperationType = OperationType.Delete;
                EntityBase.CleanUp(id);
                if (_OnPreValidateDeleteAsync != null) await _OnPreValidateDeleteAsync(id).ConfigureAwait(false);

                MultiValidator.Create()
                    .Add(id.Validate(nameof(id)).Mandatory())
                    .Additional((__mv) => _OnDeleteValidate?.Invoke(__mv, id))
                    .Run().ThrowOnError();

                if (_OnBeforeDeleteAsync != null) await _OnBeforeDeleteAsync(id).ConfigureAwait(false);
                await _dataService.DeleteAsync(id).ConfigureAwait(false);
                if (_OnAfterDeleteAsync != null) await _OnAfterDeleteAsync(id).ConfigureAwait(false);
            });
        }

        public Task<PersonCollectionResult> GetByArgsAsync(PersonArgs args, PagingArgs? pagingArgs)
        {
            return ManagerInvoker.Default.InvokeAsync(this, async () =>
            {
                ExecutionContext.Current.OperationType = OperationType.Read;
                EntityBase.CleanUp(args, pagingArgs);
                if (_OnPreValidateGetByArgsAsync != null) await _OnPreValidateGetByArgsAsync(args, pagingArgs).ConfigureAwait(false);

                MultiValidator.Create()
                    .Add(args.Validate(nameof(args)).Mandatory())
                    .Add(args.Validate(nameof(args)).Entity(PersonArgsValidator.Default))
                    .Additional((__mv) => _OnGetByArgsValidate?.Invoke(__mv, args, pagingArgs))
                    .Run().ThrowOnError();

                if (_OnBeforeGetByArgsAsync != null) await _OnBeforeGetByArgsAsync(args, pagingArgs).ConfigureAwait(false);
                var __result = await _dataService.GetByArgsAsync(args, pagingArgs).ConfigureAwait(false);
                if (_OnAfterGetByArgsAsync != null) await _OnAfterGetByArgsAsync(__result, args, pagingArgs).ConfigureAwait(false);
                Cleaner.Clean(__result);
                return __result;
            });
        }
    }
}

#nullable restore