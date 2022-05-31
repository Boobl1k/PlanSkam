import {Component} from "react";
import Track from "../../components/Track";
import {Container, Col, Row} from 'react-bootstrap'

export default class Index extends Component {
    constructor(props) {
        super(props);
        this.state = {
            author: {
                Name: '',
                Tracks: []
            }
        }
        fetch(`http://localhost:3000/authors/getWithTracks?id=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({author: JSON.parse(text)});
            }))

    }

    function App() {
        render()
        {
            return (
                <div>
                    <h1>{this.state.author.Name}</h1>
                    {this.state.author.Tracks.map(track => {
                        return <Track id={track.Id}/>
                        return <Track name={track.Name}/>
                        return <Track delete={this.delete}/>
                    })}
                </div>
            )
        }
    }
}