import logo from './logo.svg';
import './App.css';
import * as React from "react";
import { render } from 'react-dom';
import * as ra from 'react-admin';
import restProvider from 'ra-data-simple-rest';
import ClientEdit from './client/edit'
import ClientLit from './client/list'

const App=(props)=>
<ra.Admin dataProvider={restProvider('https://localhost:7000/_api')}>
  <ra.Resource name="clients/allowedScopes"/>
  <ra.Resource name="clients/claims"/>
  <ra.Resource name="clients" list={ClientLit} edit={ClientEdit} />
  <ra.Resource name="catalogs" list={ra.ListGuesser} edit={ra.EditGuesser} />

</ra.Admin>

export default App;
