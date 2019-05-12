﻿Public Class Form1

    'Variables
    Dim values As New List(Of Double)
    Dim operators As New List(Of Char)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
    End Sub
    ''' <summary>
    ''' Im String wird nach Rechenoperatoren geschaut. 
    ''' Wird einer gefunden, so wird er in die Liste für Operatoren eingetragen und sein Index in die Liste für die Indexe der Operatoren eingetragen
    ''' Der erste Wert (vor dem ersten Operator) wird in die Liste für Werte eingetragen
    ''' Alle Werte, die dann zwischen zwischen 2 Operatoren stehen, werden bestimmt und auch in die Liste für WErte eingetragen
    ''' Der letzte Wert (nach dem letzten Operator) wird auch in die Liste für Werte eingetragen
    ''' </summary>
    ''' <param name="input">Eingabe als String</param>
    Private Sub Calculate(input As String)
        Dim res As Integer
        Dim indexOfOperators As New List(Of Integer)
        indexOfOperators.Add(0)
        Dim countOfOperators As Integer
        For i = 0 To input.Length
            If i <> input.Length Then
                If input.Substring(i, 1) = "+" OrElse
                   input.Substring(i, 1) = "-" OrElse
                   input.Substring(i, 1) = "*" OrElse
                   input.Substring(i, 1) = "/" OrElse
                   input.Substring(i, 1) = "^" Then
                    countOfOperators += 1
                    indexOfOperators.Add(i)
                    operators.Add(input.Substring(i, 1))
                    If values.Count = 0 Then
                        If i = 1 Then
                            values.Add(input.Substring(0, 1))
                        Else
                            values.Add(input.Substring(0, i))
                        End If
                    Else
                        'firstIndexOfValue: der hinzugefügte Index im letzten Durchlauf +1
                        'lengthOfValue: Index des gerade hinzugefügten Index - den hinzugefügten Index aus dem letzten Durchlauf
                        Dim firstIndexOfValue = indexOfOperators(countOfOperators - 1) + 1
                        Dim lengthofValue = indexOfOperators(countOfOperators) - firstIndexOfValue
                        values.Add(input.Substring(firstIndexOfValue, lengthofValue))
                    End If
                End If
            Else
                values.Add(input.Substring(indexOfOperators.Last + 1, (i - indexOfOperators.Last) - 1))
            End If
        Next

        'Für die Punkt vor Strich Rechnung wird hier nach den Punkt Operatoren gesucht und die jeweiligen Zahlen miteinander multipliziert bzw. dividiert
        For operatorIndex = 0 To operators.Count - 1
            If getNumberOfDotOperators() = 0 Then Exit For
            If operators.Count = 0 Then Exit For
            If operators.Count = 1 Then operatorIndex = 0
            If operatorIndex >= operators.Count Then operatorIndex = operators.Count - 2
            If operators(operatorIndex) = "*" Then
                values(operatorIndex) = Multiplication(values(operatorIndex), values(operatorIndex + 1))
                RefactoringLists(operatorIndex)
            ElseIf operators(operatorIndex) = "/" Then
                values(operatorIndex) = Division(values(operatorIndex), values(operatorIndex + 1))
                RefactoringLists(operatorIndex)
            ElseIf operators(operatorIndex) = "^" Then
                values(operatorIndex) = Exponent(values(operatorIndex), values(operatorIndex + 1))
                RefactoringLists(operatorIndex)
            End If
        Next

        For operatorIndex = 0 To operators.Count - 1
            If getNumberOfDashOperators() = 0 Then Exit For
            If operators.Count = 0 Then Exit For
            If operators.Count = 1 Then operatorIndex = 0
            If operatorIndex >= operators.Count Then operatorIndex = operators.Count - 2
            If operators(operatorIndex) = "+" Then
                values(operatorIndex) = Addition(values(operatorIndex), values(operatorIndex + 1))
                RefactoringLists(operatorIndex)
            ElseIf operators(operatorIndex) = "-" Then
                values(operatorIndex) = Substraction(values(operatorIndex), values(operatorIndex + 1))
                RefactoringLists(operatorIndex)
            End If
        Next

        res = values(0)
        OutputBox.Text = res
    End Sub

    ''' <summary>
    ''' Diese Funktion wird nach dem Multiplizieren und nach dem Dividieren aufgerufen, um die Liste der Werte und der Operatoren zu kürzen
    ''' </summary>
    ''' <param name="operatorIndex">Index des Operators</param>
    Private Sub RefactoringLists(operatorIndex As Integer)
        Dim bufferoperator = operatorIndex
        While operatorIndex + 2 < values.Count
            values(operatorIndex + 1) = values(operatorIndex + 2)
            operatorIndex += 1
        End While
        values.RemoveAt(values.Count - 1)
        While bufferoperator + 1 < operators.Count
            operators(bufferoperator) = operators(bufferoperator + 1)
            bufferoperator += 1
        End While
        operators.RemoveAt(operators.Count - 1)
    End Sub

    ''' <summary>
    ''' Ermittelt die Anzahl der Punkt Operatoren in der Eingabe für den Abbruch der For Schleife für die Punkt Operatoren
    ''' </summary>
    ''' <returns></returns>
    Private Function getNumberOfDotOperators() As Integer
        Dim res = 0
        For Each element In operators
            If element = "*" OrElse element = "/" OrElse element = "^" Then res = res + 1
        Next
        Return res
    End Function

    ''' <summary>
    ''' Ermittelt die Anzahl der Strich Operatoren in der Eingabe für den Abbruch der For Schleife für die Strich Operatoren
    ''' </summary>
    ''' <returns></returns>
    Private Function getNumberOfDashOperators() As Integer
        Dim res = 0
        For Each element In operators
            If element = "+" OrElse element = "-" Then res = res + 1
        Next
        Return res
    End Function


    'Addition
    Private Function Addition(firstValue As Double, secondValue As Double) As Double
        Return firstValue + secondValue
    End Function
    'Substraktion
    Private Function Substraction(firstValue As Double, secondValue As Double) As Double
        Return firstValue - secondValue
    End Function
    'Multiplikation
    Private Function Multiplication(firstValue As Double, secondValue As Double) As Double
        Return firstValue * secondValue
    End Function
    'Division
    Private Function Division(firstValue As Double, secondValue As Double) As Double
        Return firstValue / secondValue
    End Function

    'Exponent
    Private Function Exponent(firstValue As Double, secondValue As Double) As Double
        Return firstValue ^ secondValue
    End Function


    Private Sub Modulo()

    End Sub

    Private Sub Button_Equals_Click(sender As Object, e As EventArgs) Handles Button_Equals.Click
        Dim input = inputBox.Text
        Calculate(input)
    End Sub

    ''' <summary>
    ''' Setzt alle Listen, sowie die Input und Output Box auf Nothing
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Delete_Click(sender As Object, e As EventArgs) Handles Button_Delete.Click
        operators = New List(Of Char)
        values = New List(Of Double)
        inputBox.Text = Nothing
        OutputBox.Text = Nothing
    End Sub
End Class
