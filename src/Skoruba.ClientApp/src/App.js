import logo from './logo.svg';
import './App.css';
import * as React from "react";
import { render } from 'react-dom';
import * as ra from 'react-admin';
import restProvider from 'ra-data-simple-rest';

const App=(props)=>
<ra.Admin dataProvider={restProvider('https://localhost:5000/api')}>
  <ra.Resource name="actionItems" list={ra.ListGuesser} edit={ra.EditGuesser} />
  <ra.Resource name="catalogs" list={ra.ListGuesser} edit={ra.EditGuesser} />

</ra.Admin>

export default App;
