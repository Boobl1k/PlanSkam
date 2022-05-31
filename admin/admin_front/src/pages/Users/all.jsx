import {Component} from "react";
import {NavLink} from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import {Container, Table} from "react-bootstrap";
import Button from 'react-bootstrap/Button';
import axios from "../../services/api/axios";

export default class All extends Component {
    constructor(props) {
        super(props);

        this.state = {
            users: []
        }
        this.click = this.click.bind(this);
    }

    click() {
        axios.get("/users/getAll")
            .then(res => {
                if(res.status === 200) this.setState({users: res.data});
            });
    }

    render() {
        const renderUsers = (users) => {
            console.log(users);
                return (
                    <Container>
                    <div className="d-flex justify-content-center align-items-center py-3">
                <Table striped bordered hover>
                    <thead>
                    <tr>
                        <th>Id</th>
                        <th>UserName</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
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
                    </tbody>
                </Table>
            </div>
                    </Container>)
            
        }
            

        this.click();

         
            return <Container>
            <div>
                {renderUsers(this.state.users)}
                <Button variant="outline-light" onClick={this.click}>Update</Button>{' '}
            </div>
        </Container>
    }
}
