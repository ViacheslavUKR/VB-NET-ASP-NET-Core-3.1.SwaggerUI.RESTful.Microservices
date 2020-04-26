Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.AspNetCore.Authorization
Imports Swashbuckle.AspNetCore.Annotations

<Authorize>
<Route("api/[controller]")>
<ApiController>
Public Class CustomersController
    Inherits ControllerBase

    Private ReadOnly _context As BankContext

    Public Sub New(ByVal context As BankContext)
        _context = context
    End Sub

    'GET: api/Customers
    <ProducesResponseType(StatusCodes.Status200OK)>
    <HttpGet>
    Public Function GetCustomers() As IEnumerable(Of Customer)
        Return _context.Customers
    End Function

    'GET: api/Customers/5
    <HttpGet("{id}")>
    Public Async Function GetCustomer(
    <FromRoute> ByVal id As Long) As Task(Of IActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If

        Dim customer = Await _context.Customers.FindAsync(id)

        If customer Is Nothing Then
            Return NotFound()
        End If

        Return Ok(customer)
    End Function

    'PUT: api/Customers/5
    <HttpPut("{id}")>
    Public Async Function UpdateCustomer(
    <FromRoute> ByVal id As Long,
    <FromBody> ByVal customer As Customer) As Task(Of IActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If

        If id <> customer.Id Then
            Return BadRequest()
        End If

        _context.Entry(customer).State = EntityState.Modified

        Try
            Await _context.SaveChangesAsync()
        Catch __unusedDbUpdateConcurrencyException1__ As DbUpdateConcurrencyException

            If Not CustomerExists(id) Then
                Return NotFound()
            Else
                Throw
            End If
        End Try

        Return NoContent()
    End Function

    'POST: api/Customers
    <HttpPost>
    Public Async Function CreateCustomer(
    <FromBody> ByVal customer As Customer) As Task(Of IActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If

        _context.Customers.Add(customer)
        Await _context.SaveChangesAsync()
        Return CreatedAtAction("GetCustomer", New With {
            .id = customer.Id
        }, customer)
    End Function

    'DELETE: api/Customers/5
    <HttpDelete("{id}")>
    Public Async Function DeleteCustomer(
    <FromRoute> ByVal id As Long) As Task(Of IActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If

        Dim customer = Await _context.Customers.FindAsync(id)

        If customer Is Nothing Then
            Return NotFound()
        End If

        _context.Customers.Remove(customer)
        Await _context.SaveChangesAsync()
        Return Ok(customer)
    End Function

    Private Function CustomerExists(ByVal id As Long) As Boolean
        Return _context.Customers.Any(Function(e) e.Id = id)
    End Function
End Class

