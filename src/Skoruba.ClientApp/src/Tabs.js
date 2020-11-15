import React, { Fragment, useState } from 'react';
import { withRouter, Link, Route, Switch } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
//import { Switch } from '@material-ui/core';


var styles = {
    //color:'red',
    backgroundColor: '#eeeeee',
    //fontWeight:'bold'
};

// Tabbed
const CustomTabs = ({ children, basePath, location, history }) => {
    //const [value, setValue] = useState(location.pathname);
    const [value, setValue] = useState(children[0].props.to);
    React.useEffect(() => {

        if (location.pathname === basePath) {
            const val = children[0].props.to;
            if (location.pathname !== `${basePath}/${val}`) {
                history.push(`${basePath}/${val}`);
                //setValue(val);
            }
        }
    });

    const handleChange = (event, newValue) => {
        setValue(newValue);
    }

    if (!Array.isArray(children))
        return "Tabs need multiple children";

    return (
        <Fragment>
            <div>dsldfk</div>
            <Paper  >
                <div>dfldkfj</div>
                <Tabs style={styles}
                    value={value}
                    onChange={handleChange}
                    indicatorColor="primary"
                    textColor="primary"
                >
                    {React.Children.map(children, (child, i) =>
                        //<Tab label={child.props.label}   />
                        child && <Tab label={child.props.label} 
                            component={Link} 
                            to={`${basePath}/${child.props.to}`} 
                            value={child.props.to}>
                        </Tab>
                    )}
                </Tabs>

                <Switch>
                    {React.Children.map(children, (child, i) =>
                        //<Tab label={child.props.label}   />
                        child && <Route exact={child.props.routeExact} 
                                path={`${basePath}/${child.props.to}`} 
                                render={() => {
                                    return (
                                        <React.Fragment>
                                            {child}
                                        </React.Fragment>
                                    )
                        }
                        } />
                    )}
                </Switch>
            </Paper>
        </Fragment>
    );
}


export const TabView = ({ label, to, ...props }) => {
    return (
        <React.Fragment>
            {props.children}
        </React.Fragment>
    )
};


export default withRouter(CustomTabs);