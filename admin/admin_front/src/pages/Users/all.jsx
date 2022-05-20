import {Component} from "react";
import {NavLink} from "react-router-dom";

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
                    <tr>
                        <th>Id</th>
                        <th>UserName</th>
                        <th></th>
                    </tr>
                    {
                        users.map(user => {
                            return <tr>
                                <td>{user.Id}</td>
                                <td>{user.UserName}</td>
                                <td>
                                    <NavLink to={`../user/${user.Id}`}>open</NavLink>
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
            <button onClick={this.click}>Update</button>
        </div>
    }
}
