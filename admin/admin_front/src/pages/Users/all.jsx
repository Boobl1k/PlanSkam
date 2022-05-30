import {Component} from "react";
import {NavLink} from "react-router-dom";

export default class All extends Component {
    constructor(props) {
        super(props);

        this.state = {
            users: []
        }
        this.click = this.click.bind(this);
    }

    click() {
        fetch("http://localhost:3000/users/getAll")
            .then(res => {
                if(res.status === 200) res.text().then(text => this.setState({users: JSON.parse(text)}));
            })
    }

    render() {
        const renderUsers = (users) => {
            console.log(users);
            return <div className="d-flex justify-content-center align-items-center py-3">
                <table className="d-flex justify-content-center align-items-center py-3">
                    <tr className="nav nav-pills">
                        <th>Id</th>
                        <th>UserName</th>
                        <th></th>
                    </tr>
                    {
                        users.map(user => {
                            return <tr className="nav nav-pills">
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
