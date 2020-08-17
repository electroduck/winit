Module DotNetUtil
    Public Sub ForceChangeValue(objTarget As Object, strFieldName As String, objNewValue As Object)
        Dim typeTarget As Type = objTarget.GetType
        Dim field As Reflection.FieldInfo = typeTarget.GetField(strFieldName,
            Reflection.BindingFlags.FlattenHierarchy Or Reflection.BindingFlags.GetField Or Reflection.BindingFlags.IgnoreCase _
            Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Public _
            Or Reflection.BindingFlags.Static)
        field.SetValue(objTarget, objNewValue)
    End Sub

    Public Function IsTypeOrSubclass(obj As Object, t As Type) As Boolean
        Dim typeObj As Type = obj.GetType
        Return typeObj Is t Or typeObj.IsSubclassOf(t)
    End Function
End Module
