Imports Microsoft.EntityFrameworkCore
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks


Public Class BankContext
    Inherits DbContext

    Public Sub New(ByVal options As DbContextOptions(Of BankContext))
        MyBase.New(options)
    End Sub

    Public Property Customers As DbSet(Of Customer)
End Class
