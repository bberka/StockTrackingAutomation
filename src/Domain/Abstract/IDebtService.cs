﻿using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IDebtService
{
    Result AddCustomerDebtLogRecord(DebtLog log, int authorUserId);
    Result AddSupplierDebtLogRecord(DebtLog log, int authorUserId);
    List<DebtLog> GetList();
    List<DebtLog> GetSupplierDebtLogs(int supplierId);
    List<DebtLog> GetCustomerDebtLogs(int customerId);

}