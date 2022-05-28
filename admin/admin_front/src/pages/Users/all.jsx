import {Component} from "react";
import {Button} from "@material-ui/core";

export default class All extends Component {
    constructor(props) {
        super(props);

        this.state = {
            users: [{Id: ''}]
        }
        this.click = this.click.bind(this);
    }

    click() {
        fetch("http://localhost:3000/users/getAll")
            .then(res => {
                res.text().then(text => this.setState({users: JSON.parse(text)}));
            })
    }

    render() {
        const renderUsers = (users) => {
            return <div>
                <table>
                    <tr className="nav nav-pills">
                        <th>Identifier</th>
                        <th>UserName</th>
                        <th></th>
                    </tr>
                    {
                        users.map(user => {
                            return <tr className="nav nav-pills">
                                <td>{user.Id}</td>
                                <td>{user.UserName}</td>
                                <td>
                                    <NavLink to={`../user/${user.Id}`}>OPEN</NavLink>
                                </td>
                            </tr>
                        })
                    }
                </table>
            </div>
        }

        this.click();

        return <div>
            {renderUsers(this.state.users)}
            <Button onClick={this.click}>UPDATE</Button>
        </div>
    }
}
