import * as React from "react";
import * as ra from 'react-admin';
import Tabs, { TabView } from '../Tabs';

export default ({ ...props }) => (
    <Tabs basePath={`${props.basePath}/${props.id}`} >
        <TabView label="Default" to="default"  >
            <Edit1 {...props} />
        </TabView>
        <TabView label="claims" to="claims" >
            <EditClaims {...props} />
        </TabView>
    </Tabs>
)

const Edit1 = ({ resource, ...props }) => (
    <ra.Edit {...props} resource={`${resource}`}>
        <ra.SimpleForm>
            <ra.ArrayInput source="s">
                <ra.SimpleFormIterator>
                    <ra.TextInput source="scope" />
                </ra.SimpleFormIterator>
            </ra.ArrayInput>
        </ra.SimpleForm>
    </ra.Edit>
)

const EditClaims = ({ resource, ...props }) => (
    <ra.Edit {...props} resource={`${resource}/claims`}>
        <ra.SimpleForm>
            <ra.ArrayInput source={`claims`}>
                <ra.SimpleFormIterator>
                    <ra.TextInput source="value" />
                    <ra.TextInput source="type" />
                </ra.SimpleFormIterator>
            </ra.ArrayInput>
        </ra.SimpleForm>
    </ra.Edit>
)