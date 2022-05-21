import {Component} from "react";
import Track from "../../components/Track";

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

    render() {
        return (<div>
            <h1>{this.state.author.Name}</h1>
                {this.state.author.Tracks.map(track => {
                    return <Track id={track.Id} name={track.Name} delete={this.delete}/>
                })}
            </div>)
    }
}