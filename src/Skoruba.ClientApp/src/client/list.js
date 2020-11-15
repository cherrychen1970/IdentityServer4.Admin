import * as React from "react";
import * as ra from 'react-admin';

export default (props)=>
<ra.List {...props}>
    <ra.Datagrid rowClick="edit">
        <ra.TextField source="id"/>
    </ra.Datagrid>
</ra.List>